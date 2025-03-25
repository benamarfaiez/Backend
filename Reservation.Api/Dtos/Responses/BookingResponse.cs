using Reservation.Domain.Dtos.Services;

namespace Reservation.Api.Dtos.Responses;
public record BookingResponse( int RoomId, int PersonId, DateTime BookingDate, int StartSlot, int EndSlot)
{
    public BookingResponse(BookingServiceDto value) : this(value.RoomId, value.PersonId, value.BookingDate, value.StartSlot, value.EndSlot) { }
}