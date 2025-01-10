using HotelBackend.Models;

namespace HotelBackend.DTO
{
    public class ReservationCreationResponseDto
    {
        public Reservation Reservation { get; set; }
        public PromoCode PromoCode { get; set; }
    }
}
