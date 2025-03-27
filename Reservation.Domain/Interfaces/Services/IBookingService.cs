using Reservation.Domain.Dtos.Services;
namespace Reservation.Domain.Interfaces.Services;

public interface IBookingService
{
    Task<BookingServiceDto> CreateBookingAsync(BookingServiceDto booking);
    Task<bool> DeleteBookingAsync(int bookingId);
    Task<BookingServiceDto> GetBookingByIdAsync(int bookingId);
}