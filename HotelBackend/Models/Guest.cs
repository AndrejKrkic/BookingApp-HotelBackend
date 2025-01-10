using System.Text.Json.Serialization;

namespace HotelBackend.Models
{
    public class Guest
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Veza sa rezervacijama preko pomoćne tabele
        [JsonIgnore]
        public ICollection<ReservationGuest> ReservationGuests { get; set; }
    }
}
