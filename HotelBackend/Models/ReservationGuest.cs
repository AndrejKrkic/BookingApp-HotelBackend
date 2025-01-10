namespace HotelBackend.Models
{
    public class ReservationGuest
    {
        public int ReservationId { get; set; }
        public Reservation Reservation { get; set; }

        public int GuestId { get; set; }
        public Guest Guest { get; set; }
    }
}
