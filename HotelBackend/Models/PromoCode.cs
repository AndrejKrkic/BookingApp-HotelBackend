namespace HotelBackend.Models
{
    public class PromoCode
    {
        public int Id { get; set; }
        public string Code { get; set; } // Promo kod, npr. "DISCOUNT10"
        public decimal DiscountPercentage { get; set; } // Popust u procentima (npr. 10 za 10%)
        public bool IsUsed { get; set; }
        public int GeneratedByReservationId { get; set; }
        public int UsedByReservationId { get; set; }
    }

}
