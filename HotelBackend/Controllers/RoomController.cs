using Microsoft.AspNetCore.Mvc;

namespace HotelBackend.Controllers
{
    using HotelBackend.Models;
    using HotelBackend;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {

        private readonly HotelContext _context;

        public RoomController(HotelContext context)
        {
            _context = context;
        }

        // GET: api/Room
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Room>>> GetRooms()
        {
            return await _context.Rooms.ToListAsync();
        }

        // GET: api/Room/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Room>> GetRoom(int id)
        {
            var room = await _context.Rooms.FindAsync(id);

            if (room == null)
            {
                return NotFound();
            }

            return room;
        }

        // POST: api/Room
        [HttpPost]
        public async Task<ActionResult<Room>> AddRoom(Room room)
        {
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRoom), new { id = room.Id }, room);
        }

        // PUT: api/Room/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoom(int id, Room room)
        {
            if (id != room.Id)
            {
                return BadRequest();
            }

            _context.Entry(room).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoomExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Room/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }

            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RoomExists(int id)
        {
            return _context.Rooms.Any(e => e.Id == id);
        }
    }

}
