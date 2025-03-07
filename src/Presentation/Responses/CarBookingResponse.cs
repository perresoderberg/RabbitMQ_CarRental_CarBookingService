using CarBookingService.Domain.Entities;
using System;

namespace CarBookingService.Presentation.Responses
{
    public class CarBookingResponse
    {
        public string Id { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string ProviderId { get; set; } = string.Empty;
        public CarDetails Car { get; set; } = new CarDetails();
        public string PickupLocation { get; set; } = string.Empty;
        public string DropoffLocation { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = "Pending";
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public CarBookingResponse(CarBooking booking)
        {
            Id = booking.Id;
            CustomerName = booking.CustomerName;
            CustomerEmail = booking.CustomerEmail;
            ProviderId = booking.ProviderId;
            Car = booking.Car;
            PickupLocation = booking.PickupLocation;
            DropoffLocation = booking.DropoffLocation;
            StartDate = booking.StartDate;
            EndDate = booking.EndDate;
            Status = booking.Status;
            CreatedAt = booking.CreatedAt;
            UpdatedAt = booking.UpdatedAt;
        }
    }
}
