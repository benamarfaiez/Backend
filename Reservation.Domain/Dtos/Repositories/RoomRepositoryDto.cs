using System.ComponentModel.DataAnnotations;
using Reservation.Domain.Dtos.Services;

namespace Reservation.Domain.Dtos.Repositories;

public record RoomRepositoryDto(int Id, string RoomName)
{
    public RoomRepositoryDto(RoomServiceDto value) :
        this(value.Id, value.RoomName)
    { }
}