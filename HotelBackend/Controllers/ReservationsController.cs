using HotelBackend.Models;
using HotelBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationsController : ControllerBase
    {

        private readonly IReservationService _reservationService;


        public ReservationsController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }


        [HttpPost("calculate-price")]
        public async Task<IActionResult> CalculatePrice([FromBody] ReservationRequest request)
        {
            try
            {
                var totalPrice = await _reservationService.CalculatePriceAsync(request);
                return Ok(new { TotalPrice = totalPrice });
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

        [HttpPost("create")]
        public async Task<IActionResult> CreateReservation([FromBody] ReservationRequest request)
        {
            try
            {
                var result = await _reservationService.CreateReservationAsync(request);

                return CreatedAtAction(nameof(GetReservation), new { id = result.Reservation.Id }, result);
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
        public async Task<IActionResult> GetReservation(int id)
        {
            try
            {
                var reservation = await _reservationService.GetReservationAsync(id);
                return Ok(reservation);
            }
            catch (Exception ex)
            {
                return NotFound(new ErrorResponse
                {
                    StatusCode = 404,
                    Message = "Rezervacija nije pronađena."
                });
            }
        }

        [HttpGet("occupied-dates/{roomId}")]
        public async Task<IActionResult> GetOccupiedDates(int roomId)
        {
            


            try
            {
                var dates = await _reservationService.GetOccupiedDatesAsync(roomId);
                return Ok(dates);
            }
            catch (Exception ex)
            {
                return NotFound(new ErrorResponse
                {
                    StatusCode = 404,
                    Message = ex.Message
                });
            }
        }

        [HttpPost("cancel")]
        public async Task<IActionResult> CancelReservation(int reservationId)
        {
            try
            {
                var success = await _reservationService.CancelReservationAsync(reservationId);
                return Ok(new { Message = "Reservation successfully cancelled" });
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


        //MOJ POKUSAJ
        [HttpGet("get-reservation")]
        public async Task<IActionResult> GetReservationByTokenAndEmail([FromQuery] string token, [FromQuery] string email)
        {
            var result = await _reservationService.GetReservationByTokenAndEmailAsync(token, email);

            if (result == null)
            {
                return NotFound(new ErrorResponse
                {
                    StatusCode = 404,
                    Message = "Rezervacija nije pronađena."
                });
            }

            return Ok(result);
        }

    }


}
