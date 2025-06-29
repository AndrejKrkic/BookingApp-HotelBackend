using HotelBackend.Models;

namespace HotelBackend.Services
{
    public interface IRoomService
    {
        Task<IEnumerable<Room>> GetRoomsAsync();
        Task<Room?> GetRoomAsync(int id);
        Task<Room> AddRoomAsync(Room room);
        Task<bool> UpdateRoomAsync(int id, Room room);
        Task<bool> DeleteRoomAsync(int id);
        bool RoomExists(int id);
    }
}
