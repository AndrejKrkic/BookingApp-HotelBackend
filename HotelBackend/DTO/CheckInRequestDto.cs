namespace HotelBackend.DTO
{
    public class CheckInRequestDto
    {
        public int ReservationId { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
    }
}
