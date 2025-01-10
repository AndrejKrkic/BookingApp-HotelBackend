using HotelBackend.Models;

namespace HotelBackend.DTO
{
    public class ReservationWithGuestsDto
    {
        public Reservation Reservation { get; set; }
        public List<Guest> Guests { get; set; }
    }
}
