
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CarBookingService.Application.Interfaces;
using CarBookingService.Presentation.Requests;
using CarBookingService.Application.Services;

namespace CarBookingService.Presentation.Controllers
{
    [ApiController]
    [Route("api/bookings")]
    public class CarBookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public CarBookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] CreateCarBookingRequest request)
        {
            var response = await _bookingService.CreateBookingAsync(request);
            return response == null ? BadRequest("No available car found.") : Ok(response);
        }

        [HttpGet("{orderId}/status")]
        public async Task<IActionResult> GetBookingStatus(string orderId)
        {
            var response = await _bookingService.GetBookingStatusAsync(orderId);
            return response == null ? NotFound("Booking not found") : Ok(response);
        }
    }
}
