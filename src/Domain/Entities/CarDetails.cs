namespace CarBookingService.Domain.Entities
{
    public class CarDetails
    {
        public string Make { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public double PricePerDay { get; set; }
    }
}
