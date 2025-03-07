using CarBookingService.Domain.Entities;
using CarBookingService.Presentation.Requests;
using CarBookingService.Presentation.Responses;
using System.Threading.Tasks;

namespace CarBookingService.Application.Services
{
    public interface IBookingService
    {
        Task<CarBookingResponse?> CreateBookingAsync(CreateCarBookingRequest request);
        Task<CarBookingResponse?> GetBookingStatusAsync(string orderId);
        Task UpdateBookingStatusAsync(CarBooking booking);
    }
}
