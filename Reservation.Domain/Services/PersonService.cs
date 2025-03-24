using Reservation.Domain.Dtos.Services;
using Reservation.Domain.Interfaces.Repositories;
using Reservation.Domain.Interfaces.Services;
using Reservation.Domain.Models;

namespace Reservation.Domain.Services
{
    public class PersonService(IPersonRepository personRepository) : IPersonService
    {
        public async Task<IEnumerable<PersonServiceDto>> GetPersonsAsync()
        {
            var persons = await personRepository.GetPersonsAsync();
            return persons.Select(p => new PersonServiceDto(new Person(p.Id, p.FirstName, p.LastName)));
        }

        public async Task<PersonServiceDto> GetPersonByIdAsync(int id)
        {
            var person = await personRepository.GetPersonByIdAsync(id);
            if (person == null)
            {
                return null!;
            }
            return new PersonServiceDto(new Person(person.Id, person.FirstName, person.LastName));
        }

        public async Task<PersonServiceDto> CreatePersonAsync(string firstName, string lastName)
        {
            var person = await personRepository.CreatePersonAsync(firstName, lastName);
            return new PersonServiceDto(new Person(person.Id, person.FirstName, person.LastName));
        }

        public async Task<PersonServiceDto> UpdatePersonAsync(int id, string firstName, string lastName)
        {
            var person = await personRepository.UpdatePersonAsync(id, firstName, lastName);
            if (person == null)
            {
                return null!;
            }
            return new PersonServiceDto(new Person(person.Id, person.FirstName, person.LastName));
        }

        public async Task<bool> DeletePersonAsync(int id)
        {
            return await personRepository.DeletePersonAsync(id);
        }
    }
}
