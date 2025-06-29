using HotelBackend.Models;

namespace HotelBackend.Services
{
    public interface IGuestService
    {
        Task<IEnumerable<Guest>> GetAllGuestsAsync();
        Task<Guest> GetGuestByIdAsync(int id);
    }
}
