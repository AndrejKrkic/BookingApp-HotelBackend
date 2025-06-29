using HotelBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBackend.Services
{
    public class GuestService : IGuestService
    {

        private readonly HotelContext _context;

        public GuestService(HotelContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Guest>> GetAllGuestsAsync()
        {
            return await _context.Guests.ToListAsync();
        }

        public async Task<Guest> GetGuestByIdAsync(int id)
        {
            var guest = await _context.Guests.FindAsync(id);

            if (guest == null)
            {
                throw new Exception("Guest not found");
            }

            return guest;
        }
    }
}
