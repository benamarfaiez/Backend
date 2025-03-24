using Reservation.Domain.Dtos.Services;

namespace Reservation.Api.Dtos.Responses;

public record RoomsResponse(IEnumerable<RoomResponse> Rooms)
{
    public RoomsResponse(IEnumerable<RoomServiceDto> Rooms) : this(Rooms.Select(value => new RoomResponse(value))) { }
}