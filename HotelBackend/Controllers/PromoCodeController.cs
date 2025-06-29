using HotelBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PromoCodeController : ControllerBase
    {
        private readonly IPromoCodeService _promoCodeService;


        public PromoCodeController(IPromoCodeService promoCodeService)
        {
            _promoCodeService = promoCodeService;
        }


        [HttpGet("allPromo")]
        public async Task<IActionResult> GetAllPromoCodes()
        {
            try
            {
                var result = await _promoCodeService.GetAllPromoCodesAsync();
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
        public async Task<IActionResult> GetPromoCode(int id)
        {
            try
            {
                var result = await _promoCodeService.GetPromoCodeByIdAsync(id);
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
