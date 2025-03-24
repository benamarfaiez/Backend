using Reservation.Domain.Dtos.Services;

namespace Reservation.Domain.Dtos.Repositories;

public record BookingRepositoryDto(int ReservationId, int RoomId, int PersonId, DateTime BookingDate,int StartSlot,int EndSlot)
{
    public BookingRepositoryDto(BookingServiceDto value) : 
        this(value.ReservationId, value.RoomId, value.PersonId, value.BookingDate, value.StartSlot, value.EndSlot) { }
}