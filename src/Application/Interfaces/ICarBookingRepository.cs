using CarBookingService.Domain.Entities;

namespace CarBookingService.Application.Interfaces
{
    public interface ICarBookingRepository
    {
        Task InsertBookingAsync(CarBooking booking);
        Task<CarBooking?> GetBookingByIdAsync(string id);
        Task UpdateBookingStatusAsync(string id, string status);
    }
}
