using CarBookingService.Application.Interfaces;
using CarBookingService.Domain.Entities;
using System;
using System.Threading.Tasks;
using CarBookingService.Presentation.Requests;
using CarBookingService.Presentation.Responses;

namespace CarBookingService.Application.Services
{
    public class BookingService : IBookingService
    {
        private readonly ICarBookingRepository _bookingRepository;
        private readonly IRabbitMQProducer _rabbitMQProducer;

        public BookingService(
            ICarBookingRepository bookingRepository,
            IRabbitMQProducer rabbitMQProducer)
        {
            _bookingRepository = bookingRepository;
            _rabbitMQProducer = rabbitMQProducer;
        }

        public async Task<CarBookingResponse?> CreateBookingAsync(CreateCarBookingRequest request)
        {
            var booking = new CarBooking
            {
                Id = Guid.NewGuid().ToString(),
                CustomerName = request.CustomerName,
                CustomerEmail = request.CustomerEmail,
                ProviderId = null,
                Car = null,
                PickupLocation = request.PickupLocation,
                DropoffLocation = request.DropoffLocation,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _bookingRepository.InsertBookingAsync(booking);
            _rabbitMQProducer.PublishMessage("carBookingsQueue", booking);

            return new CarBookingResponse(booking);
        }

        public async Task<CarBookingResponse?> GetBookingStatusAsync(string orderId)
        {
            var booking = await _bookingRepository.GetBookingByIdAsync(orderId);
            return booking != null ? new CarBookingResponse(booking) : null;
        }

        public async Task UpdateBookingStatusAsync(CarBooking booking)
        {
            await _bookingRepository.UpdateBookingStatusAsync(booking.Id, booking.Status);

            if (booking.Status == "Confirmed")
            {
                _rabbitMQProducer.PublishMessage("emailQueue", booking);
            }
        }
    }
}
