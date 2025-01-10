namespace HotelBackend.Models
{
    public class PromoCode
    {
        public int Id { get; set; }
        public string Code { get; set; } // Promo kod, npr. "DISCOUNT10"
        public decimal DiscountPercentage { get; set; } // Popust u procentima (npr. 10 za 10%)
        public int RoomId { get; set; } // Veza s određenom sobom
        public bool IsUsed { get; set; } // Da li je promo kod iskorišćen
    }

}
