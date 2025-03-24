using Reservation.Domain.Dtos.Services;

namespace Reservation.Api.Dtos.Responses;

public record RoomResponse(int Id, string RoomName)
{
    public RoomResponse(RoomServiceDto value) : this(value.Id, value.RoomName) { }
}