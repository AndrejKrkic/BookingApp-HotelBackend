using HotelBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBackend
{
    public class HotelContext : DbContext
    {
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<PromoCode> PromoCodes { get; set; }
        public DbSet<Guest> Guests { get; set; }
        public DbSet<ReservationGuest> ReservationGuests { get; set; }

        public HotelContext(DbContextOptions<HotelContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PromoCode>().HasData(
     new PromoCode { Id = 1, Code = "DISCOUNT10", DiscountPercentage = 10, RoomId = 1, IsUsed = false },
     new PromoCode { Id = 2, Code = "SUMMER20", DiscountPercentage = 20, RoomId = 2, IsUsed = false }
 );

            modelBuilder.Entity<ReservationGuest>()
            .HasKey(rg => new { rg.ReservationId, rg.GuestId }); // Sastavljeni primarni ključ

            modelBuilder.Entity<ReservationGuest>()
                .HasOne(rg => rg.Reservation)
                .WithMany(r => r.ReservationGuests)
                .HasForeignKey(rg => rg.ReservationId);

            modelBuilder.Entity<ReservationGuest>()
                .HasOne(rg => rg.Guest)
                .WithMany(g => g.ReservationGuests)
                .HasForeignKey(rg => rg.GuestId);
        }
    }
}
