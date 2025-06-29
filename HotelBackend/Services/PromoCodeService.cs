using HotelBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBackend.Services
{
    public class PromoCodeService : IPromoCodeService
    {
        private readonly HotelContext _context;

        public PromoCodeService(HotelContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PromoCode>> GetAllPromoCodesAsync()
        {
            return await _context.PromoCodes.ToListAsync();
        }

        public async Task<PromoCode> GetPromoCodeByIdAsync(int id)
        {
            var promoCode = await _context.PromoCodes.FindAsync(id);

            if (promoCode == null)
            {
                throw new Exception("promoCode not found");
            }

            return promoCode;
        }
    }
}
