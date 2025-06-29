using HotelBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GuestController : ControllerBase
    {
        private readonly IGuestService _guestService;


        public GuestController(IGuestService guestService)
        {
            _guestService = guestService;
        }


        [HttpGet("allGuests")]
        public async Task<IActionResult> GetAllGuests()
        {
            try
            {
                var result = await _guestService.GetAllGuestsAsync();
                return Ok(result);
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGuest(int id)
        {
            try
            {
                var result = await _guestService.GetGuestByIdAsync(id);
                return Ok(result);
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
