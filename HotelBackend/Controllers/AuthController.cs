using HotelBackend.DTO;
using HotelBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] LoginDto dto)
        {
            try
            {
                var token = await _authService.Register(dto.Email, dto.Password);
                return Ok(new { token });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            try
            {
                var token = await _authService.Login(dto.Email, dto.Password);
                return Ok(new { token });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }
    }

}
