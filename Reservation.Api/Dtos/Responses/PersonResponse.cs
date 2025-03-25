using Reservation.Domain.Dtos.Services;
namespace Reservation.Api.Dtos.Responses;

public record PersonResponse(int Id, string FirstName, string LastName)
{
    public PersonResponse(PersonServiceDto dto) : this(dto.Id, dto.FirstName, dto.LastName) { }
}
