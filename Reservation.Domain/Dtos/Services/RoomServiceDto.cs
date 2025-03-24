using Reservation.Domain.Models;

namespace Reservation.Domain.Dtos.Services;

public record RoomServiceDto(int Id, string RoomName)
{
    public RoomServiceDto(Room value) :
        this(value.Id, value.RoomName)
    { }
}