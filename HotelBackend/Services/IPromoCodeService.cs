using HotelBackend.Models;

namespace HotelBackend.Services
{
    public interface IPromoCodeService
    {
        Task<IEnumerable<PromoCode>> GetAllPromoCodesAsync();
        Task<PromoCode> GetPromoCodeByIdAsync(int id);
    }
}
