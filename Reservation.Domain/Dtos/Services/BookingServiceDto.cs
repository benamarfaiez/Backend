using Reservation.Domain.Models;
namespace Reservation.Domain.Dtos.Services;

public record BookingServiceDto(int ReservationId,int RoomId, int PersonId, DateTime BookingDate, int StartSlot, int EndSlot)
{
    public BookingServiceDto(Booking value) : 
        this(value.ReservationId, value.RoomId, value.PersonId, value.BookingDate, value.StartSlot, value.EndSlot) { }
}