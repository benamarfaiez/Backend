using Reservation.Dal.Entities;
using Reservation.Domain.Dtos.Repositories;
using Reservation.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Reservation.Dal.Repositories;

public class RoomRepository(ReservationContext context) : IRoomRepository 
{

    public async Task<IEnumerable<RoomRepositoryDto>> GetRoomsAsync()
    {
        
        var values = await context.Rooms.ToListAsync();
        return values.Select(v => new RoomRepositoryDto(v.Id, v.RoomName));
    }
    public async Task<RoomRepositoryDto> GetRoomByIdAsync(int id)
    {

        var room = await context.Rooms.FirstOrDefaultAsync(r => r.Id == id);

        if (room == null)
        {
            return null!;
        }

        return new RoomRepositoryDto(room.Id, room.RoomName);

    }

    public async Task<RoomRepositoryDto> CreateRoomAsync(string roomName)
    {
        var roomEntity = new RoomEntity { RoomName = roomName };
        context.Rooms.Add(roomEntity);
        await context.SaveChangesAsync();

        return new RoomRepositoryDto(roomEntity.Id, roomEntity.RoomName);
    }

    public async Task<RoomRepositoryDto> UpdateRoomAsync(int id, string roomName)
    {
        var roomEntity = await context.Rooms.FirstOrDefaultAsync(r => r.Id == id);
        if (roomEntity == null)
        {
            return null!;
        }

        roomEntity.RoomName = roomName;
        await context.SaveChangesAsync();

        return new RoomRepositoryDto(roomEntity.Id, roomEntity.RoomName);
    }

    public async Task<bool> DeleteRoomAsync(int id)
    {
        var roomEntity = await context.Rooms.FirstOrDefaultAsync(r => r.Id == id);
        if (roomEntity == null)
        {
            return false;
        }

        context.Rooms.Remove(roomEntity);
        await context.SaveChangesAsync();

        return true;
    }
}