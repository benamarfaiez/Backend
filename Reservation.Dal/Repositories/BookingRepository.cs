using Microsoft.EntityFrameworkCore;
using Reservation.Dal.Entities;
using Reservation.Domain.Dtos.Repositories;
using Reservation.Domain.Interfaces.Repositories;

namespace Reservation.Dal.Repositories;

public class BookingRepository(ReservationContext context) : IBookingRepository
{
    public async Task<BookingRepositoryDto> CreateBookingAsync(BookingRepositoryDto booking)
    {
        var entity = new BookingEntity
        {
            RoomId = booking.RoomId,
            PersonId = booking.PersonId,
            BookingDate = booking.BookingDate.Date,
            StartSlot = booking.StartSlot,
            EndSlot = booking.EndSlot
        };

        context.Bookings.Add(entity);
        await context.SaveChangesAsync();

        return new BookingRepositoryDto(
            entity.Id,
            entity.RoomId,
            entity.PersonId,
            entity.BookingDate,
            entity.StartSlot,
            entity.EndSlot
        );
    }
    public async Task<IEnumerable<BookingRepositoryDto>> GetBookingsByRoomAndDateAsync(int roomId, DateTime date)
    {
        return await context.Bookings
            .Where(b => b.RoomId == roomId && b.BookingDate.Date == date.Date)
            .Select(b => new BookingRepositoryDto(
                b.Id,
                b.RoomId,
                b.PersonId,
                b.BookingDate,
                b.StartSlot,
                b.EndSlot))
            .ToListAsync();
    }
    public async Task<bool> DeleteBookingAsync(int bookingId)
    {
        var booking = await context.Bookings.FindAsync(bookingId);
        if (booking == null)
        {
            return false;
        }

        context.Bookings.Remove(booking);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<BookingRepositoryDto?> GetBookingByIdAsync(int bookingId)
    {
        var booking = await context.Bookings.FindAsync(bookingId);
        if (booking == null)
        {
            return null;
        }

        return new BookingRepositoryDto(
            booking.Id,
            booking.RoomId,
            booking.PersonId,
            booking.BookingDate,
            booking.StartSlot,
            booking.EndSlot
        );
    }

    public async Task<IEnumerable<BookingRepositoryDto>> GetConflictingBookingsAsync(int roomId, DateTime bookingDate, int startSlot, int endSlot)
    {
        return await context.Bookings
            .Where(b => b.RoomId == roomId &&
                  b.BookingDate.Date == bookingDate.Date &&
                  ((b.StartSlot <= startSlot && b.EndSlot > startSlot) ||
                   (b.StartSlot < endSlot && b.EndSlot >= endSlot) ||
                   (b.StartSlot >= startSlot && b.EndSlot <= endSlot)))
            .Select(b => new BookingRepositoryDto(
                b.Id,
                b.RoomId,
                b.PersonId,
                b.BookingDate,
                b.StartSlot,
                b.EndSlot))
            .ToListAsync();
    }
}