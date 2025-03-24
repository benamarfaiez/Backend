﻿using Reservation.Domain.Dtos.Repositories;
using Reservation.Domain.Dtos.Services;
using Reservation.Domain.Interfaces.Repositories;
using Reservation.Domain.Interfaces.Services;
using Reservation.Domain.Models;

namespace Reservation.Domain.Services;

public class BookingService(IBookingRepository bookingRepository) : IBookingService
{
    public async Task<BookingServiceDto> CreateBookingAsync(BookingServiceDto booking)
    {
        try
        {
            await ValidateBookingAsync(booking);
            var repositoryDto = new BookingRepositoryDto(booking);
            var result = await bookingRepository.CreateBookingAsync(repositoryDto);
            return new BookingServiceDto(
                new Booking(
                    result.ReservationId,
                    result.RoomId,
                    result.PersonId,
                    result.BookingDate,
                    result.StartSlot,
                    result.EndSlot
                )
            );
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("créneau horaire"))
        {
            // Si l'exception est due à un conflit de réservation, récupérer les créneaux disponibles
            var availableSlots = await GetAvailableSlotsForDay(booking.RoomId, booking.BookingDate);
            throw new Exception(
                "Ce créneau horaire est déjà réservé pour cette salle. Voici les créneaux disponibles pour cette journée."
            );
        }
    }

    private async Task<List<SlotDto>> GetAvailableSlotsForDay(int roomId, DateTime date)
    {
        // Récupérer toutes les réservations pour cette salle et cette date
        var existingBookings = await bookingRepository.GetBookingsByRoomAndDateAsync(roomId, date);

        // Supposons que les créneaux valides sont de 1 à 24 (pour simplifier)
        var allSlots = Enumerable.Range(1, 24);

        // Créer une liste de tous les créneaux horaires occupés
        var occupiedSlots = new HashSet<int>();
        foreach (var booking in existingBookings)
        {
            // Ajouter tous les créneaux entre StartSlot et EndSlot
            for (int slot = booking.StartSlot; slot < booking.EndSlot; slot++)
            {
                occupiedSlots.Add(slot);
            }
        }

        // Déterminer les créneaux libres
        var availableSlots = new List<SlotDto>();
        int? startSlot = null;

        foreach (var slot in allSlots)
        {
            if (!occupiedSlots.Contains(slot))
            {
                // Si on n'a pas encore commencé un créneau libre, on commence maintenant
                if (startSlot == null)
                {
                    startSlot = slot;
                }
            }
            else
            {
                // Si on avait commencé un créneau libre et qu'on trouve un créneau occupé, 
                // on ajoute le créneau libre à la liste
                if (startSlot != null)
                {
                    availableSlots.Add(new SlotDto(startSlot.Value, slot));
                    startSlot = null;
                }
            }
        }

        // Si on finit sur un créneau libre, on l'ajoute aussi
        if (startSlot != null)
        {
            availableSlots.Add(new SlotDto(startSlot.Value, 25)); // 25 représente la fin de la journée
        }

        return availableSlots;
    }

    public async Task<bool> DeleteBookingAsync(int bookingId)
    {
        return await bookingRepository.DeleteBookingAsync(bookingId);
    }

    private async Task ValidateBookingAsync(BookingServiceDto booking)
    {
        if (booking.BookingDate.Date < DateTime.Today)
        {
            throw new InvalidOperationException("La date de réservation doit être une date future.");
        }

        if (booking.StartSlot >= booking.EndSlot)
        {
            throw new InvalidOperationException("L'heure de début doit être avant l'heure de fin.");
        }

        if (booking.StartSlot < 1 || booking.StartSlot > 24 || booking.EndSlot < 1 || booking.EndSlot > 24)
        {
            throw new InvalidOperationException("Les créneaux horaires doivent être entre 1 et 24.");
        }

        var conflictingBookings = await bookingRepository.GetConflictingBookingsAsync(
            booking.RoomId,
            booking.BookingDate,
            booking.StartSlot,
            booking.EndSlot
        );

        if (conflictingBookings.Any())
        {
            throw new InvalidOperationException("Ce créneau horaire est déjà réservé pour cette salle.");
        }
    }

}

internal record SlotDto(int startSlot, int endtSlot);