using Reservation.Domain.Dtos.Services;
namespace Reservation.Domain.Dtos.Repositories;

public record PersonRepositoryDto(int Id, string FirstName, string LastName)
{
    public PersonRepositoryDto(PersonServiceDto value) :
        this(value.Id, value.FirstName, value.LastName)
    { }

}