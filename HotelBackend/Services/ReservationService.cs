using HotelBackend.DTO;
using HotelBackend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;

namespace HotelBackend.Services
{
    public class ReservationService : IReservationService
    {
        private readonly HotelContext _context;

        public ReservationService(HotelContext context)
        {
            _context = context;
        }

        public async Task<decimal> CalculatePriceAsync(ReservationRequest request)
        {
            var room = await _context.Rooms.FindAsync(request.RoomId);
            if (room == null)
                throw new Exception("Room not found");

            var numberOfNights = (request.CheckOutDate - request.CheckInDate).Days;
            if (numberOfNights <= 0)
                throw new Exception("Invalid dates");

            var totalPrice = numberOfNights * room.PricePerNight;

            if (!string.IsNullOrEmpty(request.PromoCode))
            {
                var promo = await _context.PromoCodes
                    .FirstOrDefaultAsync(p => p.Code == request.PromoCode && !p.IsUsed);

                if (promo == null)
                    throw new Exception("Invalid promo code");

                totalPrice -= totalPrice * (promo.DiscountPercentage / 100);
            }

            return totalPrice;
        }


        public async Task<ReservationCreationResponseDto> CreateReservationAsync(ReservationRequest request)
        {
            // Pronađi sobu prema prosleđenom ID-u
            var room = await _context.Rooms.FindAsync(request.RoomId);
            if (room == null)
                throw new Exception("Soba nije pronađena.");
            
            // Izračunaj broj noćenja
            var numberOfNights = (request.CheckOutDate - request.CheckInDate).Days;
            if (numberOfNights <= 0)
                throw new Exception("Neispravni datumi.");

            if (request.GuestNames.IsNullOrEmpty())
                throw new Exception("Prijava mora sadržati bar jednog gosta.");
           

            // Proveri da li postoji preklapanje sa postojećim rezervacijama

            //proverava da li je unet check in bio nakon check outa bilo koje sacuvane rezervacije u bazi, i pokriva slucaj ako je ckek in bio pre check outa ali je i unet check out bio pre bilo kog check ina, pa nema preklapanja.
            var overlappingReservations = await _context.Reservations
                .Where(r => r.RoomId == request.RoomId &&
                            !(r.CheckOut <= request.CheckInDate || r.CheckIn >= request.CheckOutDate)) // Nema preklapanja, proverava disjunkciju
                .ToListAsync();

            if (overlappingReservations.Any())
                throw new Exception("Soba je već rezervisana za izabrane dane.");

            // Izračunaj osnovnu cenu
            decimal totalPrice = numberOfNights * room.PricePerNight;

            PromoCode usedPromoCode = new PromoCode();

            // Ako je unet promo kod, primeni popust
            if (!string.IsNullOrEmpty(request.PromoCode))
            {
                var promo = await _context.PromoCodes
                    .FirstOrDefaultAsync(p => p.Code == request.PromoCode && !p.IsUsed);

                if (promo == null)
                    throw new Exception("Neispravan ili nepostojeći promo kod.");

                // Primenjujemo popust
                totalPrice -= totalPrice * (promo.DiscountPercentage / 100);
                promo.IsUsed = true; // Obeležimo promo kod kao iskorišćen
                

                usedPromoCode = promo;
                _context.PromoCodes.Update(promo);
            }

            // Generiši nasumični token (na osnovu GUID-a)
            string token = Guid.NewGuid().ToString();


            // Definiši moguće vrednosti popusta
            var possibleDiscounts = new[] { 5, 10, 15, 20 };

            // Generiši nasumičan indeks iz niza mogućih vrednosti
            var random = new Random();
            var discountPercentage = possibleDiscounts[random.Next(possibleDiscounts.Length)]; // Nasumično biraj iz niza

            // Generiši novi promo kod
            var promoCode = new PromoCode
            {
                Code = Guid.NewGuid().ToString().Substring(0, 8).ToUpper(), // Kratki, nasumični kod
                DiscountPercentage = discountPercentage,
                IsUsed = false
            };

            _context.PromoCodes.Add(promoCode); //nakon dodavanja u context on sam dopuni ID objektu promoCode
            await _context.SaveChangesAsync();


            // Kreiraj novu rezervaciju
            var reservation = new Reservation
            {
                RoomId = request.RoomId,
                CheckIn = request.CheckInDate,
                CheckOut = request.CheckOutDate,
                TotalPrice = totalPrice,
                Email = request.Email,
                Token = token, // Dodeljujemo generisani token
                Status = "Active",
                GeneratedPromoCode = promoCode.Id,
                UsedPromoCodeId = usedPromoCode.Id
            };

            // Dodaj rezervaciju u kontekst
            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            promoCode.GeneratedByReservationId = reservation.Id;
            await _context.SaveChangesAsync();

            usedPromoCode.UsedByReservationId = reservation.Id;
            await _context.SaveChangesAsync();

            // Kreiraj goste na osnovu liste imena i poveži ih sa rezervacijom
            foreach (var guestName in request.GuestNames)
            {
                Console.WriteLine("Gost je " + guestName);
                var guest = new Guest
                {
                    Name = guestName
                };

                _context.Guests.Add(guest);
                await _context.SaveChangesAsync(); // Dodaj novog gosta

                // Kreiraj vezu između gosta i rezervacije
                var guestReservation = new ReservationGuest
                {
                    GuestId = guest.Id,
                    ReservationId = reservation.Id
                };
                _context.ReservationGuests.Add(guestReservation);
                await _context.SaveChangesAsync();
            }

            return new ReservationCreationResponseDto
            {
                Reservation = reservation,
                PromoCode = promoCode
            };
        }

        public async Task<Reservation> GetReservationAsync(int id)
        {
            return await _context.Reservations
                .Include(r => r.ReservationGuests)
                .ThenInclude(rg => rg.Guest)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<List<DateTime>> GetOccupiedDatesAsync(int roomId)
        {
            // Proveri da li soba postoji
            var room = await _context.Rooms.FindAsync(roomId);
            if (room == null)
                throw new Exception("Room not found");
            

            var reservations = await _context.Reservations
                .Where(r => r.RoomId == roomId)
                .ToListAsync();

            return reservations.SelectMany(r => Enumerable.Range(0, (r.CheckOut - r.CheckIn).Days + 1)
                .Select(offset => r.CheckIn.AddDays(offset)))
                .ToList();
        }

        public async Task<bool> CancelReservationAsync(int reservationId)
        {
            var reservation = await _context.Reservations.FindAsync(reservationId);
            if (reservation == null)
                throw new Exception("Reservation not found");

            if ((reservation.CheckIn - DateTime.UtcNow).TotalDays < 5)
                throw new Exception("Cancellation is only allowed at least 5 days before check-in");

            reservation.Status = "Cancelled";
            _context.Reservations.Update(reservation);
            await _context.SaveChangesAsync();

            var promoCode = await _context.PromoCodes.FindAsync(reservation.GeneratedPromoCode);
            if(promoCode != null)
            {
                promoCode.IsUsed = true;
                await _context.SaveChangesAsync();
            }
            
            return true;
        }

        public async Task<ReservationWithGuestsDto> GetReservationByTokenAndEmailAsync(string token, string email)
        {
            // Pronađi rezervaciju na osnovu tokena i email-a
            var reservation = await _context.Reservations
                .Include(r => r.ReservationGuests)  // Učitaj povezane goste
                .ThenInclude(rg => rg.Guest)      // Učitaj goste
                .FirstOrDefaultAsync(r => r.Token == token && r.Email == email);

            if (reservation == null)
            {
                return null; // Možeš rukovati greškom kasnije, u kontroleru.
            }

            // Kreiraj DTO koji uključuje rezervaciju i goste
            var result = new ReservationWithGuestsDto
            {
                Reservation = reservation,
                Guests = reservation.ReservationGuests.Select(rg => rg.Guest).ToList() // Lista gostiju vezanih za rezervaciju
            };

            return result;
        }
    }
}
