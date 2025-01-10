using HotelBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace HotelBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationsController : ControllerBase
    {
        private readonly HotelContext _context;

        public ReservationsController(HotelContext context)
        {
            _context = context;
        }

        [HttpPost("calculate-price")]
        public async Task<IActionResult> CalculatePrice([FromBody] ReservationRequest request)
        {
            // Pronađi sobu na osnovu RoomId
            var room = await _context.Rooms.FindAsync(request.RoomId);
            if (room == null)
            {
                return NotFound("Room not found :(");
            }

            // Izračunaj broj noćenja
            var numberOfNights = (request.CheckOutDate - request.CheckInDate).Days;
            if (numberOfNights <= 0)
            {
                var errorResponse = new ErrorResponse
                {
                    StatusCode = 404,
                    Message = "Neispravni datumi.",
                };

                return NotFound(errorResponse);
            }

            // Početna cena bez popusta
            var totalPrice = numberOfNights * room.PricePerNight;

            // Ako je unet promo kod, proveri ga
            if (!string.IsNullOrEmpty(request.PromoCode))
            {
                var promo = await _context.PromoCodes
                    .FirstOrDefaultAsync(p => p.Code == request.PromoCode && p.RoomId == request.RoomId && !p.IsUsed);

                if (promo == null)
                {
                    var errorResponse = new ErrorResponse
                    {
                        StatusCode = 404,
                        Message = "Neispravan ili nepostojeći promo kod.",
                    };

                    return NotFound(errorResponse);
                }

                // Primeni popust
                totalPrice -= totalPrice * (promo.DiscountPercentage / 100);
            }

            return Ok(new { TotalPrice = totalPrice });
        }


        [HttpPost("create")]
        public async Task<IActionResult> CreateReservation([FromBody] ReservationRequest request)
        {
            // Pronađi sobu prema prosleđenom ID-u
            var room = await _context.Rooms.FindAsync(request.RoomId);
            if (room == null)
            {
                //return NotFound("Room not found");
                var errorResponse = new ErrorResponse
                {
                    StatusCode = 404,
                    Message = "Soba nije pronađena.",
                };

                return NotFound(errorResponse); // ASP.NET će automatski serijalizovati u JSON
            }

            // Izračunaj broj noćenja
            var numberOfNights = (request.CheckOutDate - request.CheckInDate).Days;
            if (numberOfNights <= 0)
            {
                //return BadRequest("Invalid dates");
                var errorResponse = new ErrorResponse
                {
                    StatusCode = 404,
                    Message = "Neispravni datumi.",
                };

                return NotFound(errorResponse); 
            }

            if (request.GuestNames.IsNullOrEmpty())
            {
                //return BadRequest("Must contain one guest!");
                var errorResponse = new ErrorResponse
                {
                    StatusCode = 404,
                    Message = "Prijava mora sadržati bar jednog gosta.",
                };

                return NotFound(errorResponse); 
            }

            // Proveri da li postoji preklapanje sa postojećim rezervacijama
            var overlappingReservations = await _context.Reservations
                .Where(r => r.RoomId == request.RoomId &&
                            !(r.CheckOut <= request.CheckInDate || r.CheckIn >= request.CheckOutDate)) // Nema preklapanja, proverava disjunkciju
                .ToListAsync();

            if (overlappingReservations.Any())
            {
                //return Conflict("The selected room is already reserved for the chosen dates.");
                var errorResponse = new ErrorResponse
                {
                    StatusCode = 404,
                    Message = "Soba je već rezervisana za izabrane dane.",
                };

                return NotFound(errorResponse); 
            }

            // Izračunaj osnovnu cenu
            decimal totalPrice = numberOfNights * room.PricePerNight;

            // Ako je unet promo kod, primeni popust
            if (!string.IsNullOrEmpty(request.PromoCode))
            {
                var promo = await _context.PromoCodes
                    .FirstOrDefaultAsync(p => p.Code == request.PromoCode && p.RoomId == request.RoomId && !p.IsUsed);

                if (promo == null)
                {
                    //return BadRequest("Invalid or expired promo code.");
                    var errorResponse = new ErrorResponse
                    {
                        StatusCode = 404,
                        Message = "Neispravan ili nepostojeći promo kod.",
                    };

                    return NotFound(errorResponse); // ASP.NET će automatski serijalizovati u JSON
                }

                // Primenjujemo popust
                totalPrice -= totalPrice * (promo.DiscountPercentage / 100);
                promo.IsUsed = true; // Obeležimo promo kod kao iskorišćen
                _context.PromoCodes.Update(promo);
            }

            // Generiši nasumični token (na osnovu GUID-a)
            string token = Guid.NewGuid().ToString();

            // Kreiraj novu rezervaciju
            var reservation = new Reservation
            {
                RoomId = request.RoomId,
                CheckIn = request.CheckInDate,
                CheckOut = request.CheckOutDate,
                TotalPrice = totalPrice,
                Email = request.Email,
                Token = token, // Dodeljujemo generisani token
                Status = "Active"
            };

            // Dodaj rezervaciju u kontekst
            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            // Kreiraj goste na osnovu liste imena i poveži ih sa rezervacijom

            

            foreach (var guestName in request.GuestNames)
            {
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
            }

            // Generiši novi promo kod za istu sobu
            //var random = new Random();
            //var discountPercentage = random.Next(5, 21); // Nasumično generiši popust (5%, 10%, 15%, 20%)
            //var promoCode = new PromoCode
            //{
            //    Code = Guid.NewGuid().ToString().Substring(0, 8).ToUpper(), // Kratki, nasumični kod
            //    DiscountPercentage = discountPercentage,
            //    RoomId = request.RoomId,
            //    IsUsed = false
            //};
            //_context.PromoCodes.Add(promoCode);

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
                RoomId = request.RoomId,
                IsUsed = false
            };

            // Sačuvaj promene u bazi
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReservation), new { id = reservation.Id }, new
            {
                Reservation = reservation,
                GeneratedPromoCode = new
                {
                    Code = promoCode.Code,
                    DiscountPercentage = promoCode.DiscountPercentage
                }
            });
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetReservation(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);

            if (reservation == null)
            {
                return NotFound("Reservation not found.");
            }

            return Ok(reservation);
        }


        [HttpGet("get-reservation")]
        public async Task<IActionResult> GetReservationByTokenAndEmail([FromQuery] string token, [FromQuery] string email)
        {
            // Pronađi rezervaciju na osnovu tokena i email-a
            var reservation = await _context.Reservations
                .Include(r => r.ReservationGuests)  // Učitaj povezane goste
                .ThenInclude(rg => rg.Guest)      // Učitaj goste
                .FirstOrDefaultAsync(r => r.Token == token && r.Email == email);

            if (reservation == null)
            {
                //return NotFound("Reservation not found.");
                var errorResponse = new ErrorResponse
                {
                    StatusCode = 404,
                    Message = "Rezervacija nije pronadjena.",
                };

                return NotFound(errorResponse);
            }

            // Kreiraj rezultat koji uključuje rezervaciju i goste
            var result = new
            {
                Reservation = reservation,
                Guests = reservation.ReservationGuests.Select(rg => rg.Guest).ToList() // Lista gostiju vezanih za rezervaciju
            };

            return Ok(result);
        }



        [HttpGet("occupied-dates/{roomId}")]
        public async Task<IActionResult> GetOccupiedDates(int roomId)
        {
            // Proveri da li soba postoji
            var room = await _context.Rooms.FindAsync(roomId);
            if (room == null)
            {
                return NotFound("Room not found");
            }

            // Pronađi sve rezervacije za tu sobu
            var reservations = await _context.Reservations
                .Where(r => r.RoomId == roomId)
                .ToListAsync();

            // Generiši listu zauzetih datuma
            var occupiedDates = new List<DateTime>();
            foreach (var reservation in reservations)
            {
                var dates = Enumerable
                    .Range(0, (reservation.CheckOut - reservation.CheckIn).Days + 1) //Ovde smo dodali 1 da ne moze da se rezervise ni na check out danu
                    .Select(offset => reservation.CheckIn.AddDays(offset))
                    .ToList();
                occupiedDates.AddRange(dates);
            }

            // Vrati rezultat
            return Ok(occupiedDates);
        }

        [HttpPost("cancel")]
        public async Task<IActionResult> CancelReservation(int reservationId)
        {
            var reservation = await _context.Reservations.FindAsync(reservationId);
            if (reservation == null)
            {
                return NotFound("Reservation not found.");
            }

            // Proveri da li je otkazivanje moguće
            if ((reservation.CheckIn - DateTime.UtcNow).TotalDays < 5)
            {
                //return BadRequest("Cancellation is only allowed at least 5 days before the check-in date.");
                var errorResponse = new ErrorResponse
                {
                    StatusCode = 404,
                    Message = "Mozete otkazati samo minimum 5 dana pre check-in datuma!",
                };

                return NotFound(errorResponse);
            }

            // Postavi status na "otkazana"
            reservation.Status = "Cancelled";
            _context.Reservations.Update(reservation);
            await _context.SaveChangesAsync();

            return Ok("Reservation successfully cancelled.");
        }

    }


}
