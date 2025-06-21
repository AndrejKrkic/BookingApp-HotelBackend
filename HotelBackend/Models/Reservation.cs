using System.Text.Json.Serialization;

namespace HotelBackend.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public Room Room { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public decimal TotalPrice { get; set; }
        public string Email {  get; set; }
        public string Token { get; set; }
        public string Status { get; set; }
        // Veza sa gostima preko pomoćne tabele
        [JsonIgnore]  // Ignoriši vezu za serijalizaciju
        public ICollection<ReservationGuest> ReservationGuests { get; set; }
        public int UsedPromoCodeId { get; set; }
        public int GeneratedPromoCode { get; set; }
    }
}
