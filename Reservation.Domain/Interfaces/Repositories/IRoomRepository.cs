using Reservation.Domain.Dtos.Repositories;

namespace Reservation.Domain.Interfaces.Repositories;
public interface IRoomRepository
{
    Task<IEnumerable<RoomRepositoryDto>> GetRoomsAsync();
    Task<RoomRepositoryDto> GetRoomByIdAsync(int id);
    Task<RoomRepositoryDto> CreateRoomAsync(string roomName);
    Task<RoomRepositoryDto> UpdateRoomAsync(int id, string roomName);
    Task<bool> DeleteRoomAsync(int id);

}