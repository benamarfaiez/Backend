using Reservation.Domain.Dtos.Services;

namespace Reservation.Api.Dtos.Responses;

public record PersonsResponse(IEnumerable<PersonResponse> Persons)
{
    public PersonsResponse(IEnumerable<PersonServiceDto> Persons) : this(Persons.Select(value => new PersonResponse(value))) { }
}
