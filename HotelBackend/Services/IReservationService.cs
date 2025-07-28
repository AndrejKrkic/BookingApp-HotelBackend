using HotelBackend.DTO;
using HotelBackend.Models;

namespace HotelBackend.Services
{
    public interface IReservationService
    {
        Task<decimal> CalculatePriceAsync(ReservationRequest request);
        Task<ReservationCreationResponseDto> CreateReservationAsync(ReservationRequest request);
        Task<Reservation> GetReservationAsync(int id);
        Task<List<DateTime>> GetOccupiedDatesAsync(int roomId);
        Task<bool> CancelReservationAsync(int reservationId);
        Task<ReservationWithGuestsDto> GetReservationByTokenAndEmailAsync(string token, string email);
        Task<List<ReservationWithGuestsDto>> GetReservationsForLoggedInUserAsync();
        Task<bool> CheckInAsync(int reservationId, string imageUrl);
        Task<List<Reservation>> GetAllReservationsAsync();
    }
}
