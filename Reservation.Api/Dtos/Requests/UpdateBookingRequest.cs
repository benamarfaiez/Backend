namespace Reservation.Api.Dtos.Requests;
public record UpdateBookingRequest(int roomId,int personId, DateTime bookingDate, int startSlot, int endSlot);