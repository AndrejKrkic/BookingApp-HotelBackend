namespace HotelBackend.Models
{
    public class Room
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public decimal PricePerNight { get; set; }
        public bool IsAvailable { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }
}
