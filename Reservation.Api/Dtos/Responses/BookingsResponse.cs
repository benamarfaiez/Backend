using Reservation.Domain.Dtos.Services;

namespace Reservation.Api.Dtos.Responses;

public record BookingsResponse(IEnumerable<BookingResponse> Values)
{
    public BookingsResponse(IEnumerable<BookingServiceDto> values) : this(values.Select(value => new BookingResponse(value))) { }
}