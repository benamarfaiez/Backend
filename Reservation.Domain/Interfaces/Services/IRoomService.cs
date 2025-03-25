using Reservation.Domain.Dtos.Services;
namespace Reservation.Domain.Interfaces.Services;
public interface IRoomService
{
    Task<RoomServiceDto> GetRoomByIdAsync(int id);
    Task<IEnumerable<RoomServiceDto>> GetRoomsAsync();

    Task<RoomServiceDto> CreateRoomAsync(string roomName);
    Task<RoomServiceDto> UpdateRoomAsync(int id, string roomName);
    Task<bool> DeleteRoomAsync(int id);
}
