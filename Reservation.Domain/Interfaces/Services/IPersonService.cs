using Reservation.Domain.Dtos.Services;

namespace Reservation.Domain.Interfaces.Services;
public interface IPersonService
{
    Task<IEnumerable<PersonServiceDto>> GetPersonsAsync();
    Task<PersonServiceDto> GetPersonByIdAsync(int id);
    Task<PersonServiceDto> CreatePersonAsync(string firstName, string lastName);
    Task<PersonServiceDto> UpdatePersonAsync(int id, string firstName, string lastName);
    Task<bool> DeletePersonAsync(int id);
}
