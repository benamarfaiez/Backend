namespace Reservation.Domain.Models;

public record Booking(int ReservationId, int RoomId, int PersonId, DateTime BookingDate, int StartSlot, int EndSlot);