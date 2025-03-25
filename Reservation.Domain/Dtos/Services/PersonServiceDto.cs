using Reservation.Domain.Models;
namespace Reservation.Domain.Dtos.Services;

public record PersonServiceDto(int Id, string FirstName, string LastName)
{
    public PersonServiceDto(Person value) :
        this(value.Id, value.FirstName, value.LastName)
    { }
}