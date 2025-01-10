using HotelBackend.Models;

namespace HotelBackend
{
    public class ReservationRequest
    {
        public int RoomId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public string Email { get; set; }
        public string PromoCode { get; set; } // Promo kod
                                             
        public List<string> GuestNames { get; set; }  // Lista gostiju (samo imena)
    }
}
