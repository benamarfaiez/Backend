using Reservation.Dal.Entities;
using Reservation.Domain.Dtos.Repositories;
using Reservation.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Reservation.Dal.Repositories;

public class PersonRepository(ReservationContext context) : IPersonRepository
{


    public async Task<IEnumerable<PersonRepositoryDto>> GetPersonsAsync()
    {
        var values = await context.People.ToListAsync();
        return values.Select(v => new PersonRepositoryDto(v.Id, v.FirstName, v.LastName));
    }

    public async Task<PersonRepositoryDto> GetPersonByIdAsync(int id)
    {
        var person = await context.People.FirstOrDefaultAsync(p => p.Id == id);
        if (person == null)
        {
            return null!;
        }
        return new PersonRepositoryDto(person.Id, person.FirstName, person.LastName);
    }

    public async Task<PersonRepositoryDto> CreatePersonAsync(string firstName, string lastName)
    {
        var personEntity = new PersonEntity { FirstName = firstName, LastName = lastName };
        context.People.Add(personEntity);
        await context.SaveChangesAsync();
        return new PersonRepositoryDto(personEntity.Id, personEntity.FirstName, personEntity.LastName);
    }

    public async Task<PersonRepositoryDto> UpdatePersonAsync(int id, string firstName, string lastName)
    {
        var personEntity = await context.People.FirstOrDefaultAsync(p => p.Id == id);
        if (personEntity == null)
        {
            return null!;
        }
        personEntity.FirstName = firstName;
        personEntity.LastName = lastName;
        await context.SaveChangesAsync();
        return new PersonRepositoryDto(personEntity.Id, personEntity.FirstName, personEntity.LastName);
    }

    public async Task<bool> DeletePersonAsync(int id)
    {
        var personEntity = await context.People.FirstOrDefaultAsync(p => p.Id == id);
        if (personEntity == null)
        {
            return false;
        }
        context.People.Remove(personEntity);
        await context.SaveChangesAsync();
        return true;
    }

}