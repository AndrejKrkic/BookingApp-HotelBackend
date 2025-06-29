using HotelBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBackend.Services
{
    public class RoomService : IRoomService
    {

        private readonly HotelContext _context;

        public RoomService(HotelContext context)
        {
            _context = context;
        }


        public async Task<Room> AddRoomAsync(Room room)
        {
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();
            return room;
        }

        public async Task<bool> DeleteRoomAsync(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                throw new Exception("room not found");
            }

            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Room?> GetRoomAsync(int id)
        {
            var room = await _context.Rooms.FindAsync(id);

            if (room == null)
            {
                throw new Exception("Room not found");
            }

            return room;
        }

        public async Task<IEnumerable<Room>> GetRoomsAsync()
        {
            return await _context.Rooms.ToListAsync();
        }

        public bool RoomExists(int id)
        {
            return _context.Rooms.Any(e => e.Id == id);
        }

        public async Task<bool> UpdateRoomAsync(int id, Room room)
        {

            if (id != room.Id)
            {
                throw new Exception("Soba nije pronadjena!");
            }

            _context.Entry(room).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoomExists(id)) return false;
                throw;
            }

        }
    }
}
