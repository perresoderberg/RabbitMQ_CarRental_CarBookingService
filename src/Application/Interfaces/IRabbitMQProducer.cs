namespace CarBookingService.Application.Interfaces
{
    public interface IRabbitMQProducer
    {
        void PublishMessage(string queue, object message);
    }
}
