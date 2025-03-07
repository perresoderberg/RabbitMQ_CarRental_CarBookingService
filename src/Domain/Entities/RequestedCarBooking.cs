using System;
using System.ComponentModel.DataAnnotations;

namespace CarBookingService.Domain.Entities
{
    public class RequestedCarBooking
    {
        [Required]
        public string CustomerName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string CustomerEmail { get; set; } = string.Empty;

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Max price must be greater than 0.")]
        public double MaxPricePerDay { get; set; }  // Budget for rental

        [Required]
        public string PickupLocation { get; set; } = string.Empty;

        [Required]
        public string DropoffLocation { get; set; } = string.Empty;

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }
    }
}
