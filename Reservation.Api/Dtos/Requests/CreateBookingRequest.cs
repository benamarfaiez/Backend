namespace Reservation.Api.Dtos.Requests;
public record CreateBookingRequest(int RoomId, int PersonId, DateTime BookingDate, int StartSlot, int EndSlot);