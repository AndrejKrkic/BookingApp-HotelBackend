using Microsoft.AspNetCore.Mvc;

namespace HotelBackend.Controllers
{
    using HotelBackend.Models;
    using HotelBackend;
    using Microsoft.AspNetCore.Mvc;
    using HotelBackend.Services;

    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {

        private readonly IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        // GET: api/Room
        [HttpGet]
        public async Task<IActionResult> GetRooms()
        {
            try
            {
                var rooms = await _roomService.GetRoomsAsync();
                return Ok(rooms);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse
                {
                    StatusCode = 400,
                    Message = ex.Message
                });
            }
        }

        // GET: api/Room/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoom(int id)
        {
            try
            {
                var room = await _roomService.GetRoomAsync(id);
                return Ok(room);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse
                {
                    StatusCode = 400,
                    Message = ex.Message
                });
            }
        }

        // POST: api/Room
        [HttpPost]
        public async Task<IActionResult> AddRoom(Room room)
        {
            try
            {
                var addedRoom = await _roomService.AddRoomAsync(room);
                return Ok(addedRoom);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse
                {
                    StatusCode = 400,
                    Message = ex.Message
                });
            }
        }

        // PUT: api/Room/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoom(int id, Room room)
        {
            try
            {
                await _roomService.UpdateRoomAsync(id, room);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse
                {
                    StatusCode = 400,
                    Message = ex.Message
                });
            }
        }

        // DELETE: api/Room/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            try
            {
                await _roomService.DeleteRoomAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse
                {
                    StatusCode = 400,
                    Message = ex.Message
                });
            }
        }
    }

}
