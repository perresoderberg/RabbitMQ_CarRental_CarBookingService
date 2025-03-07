using CarBookingService.Application.Interfaces;
using CarBookingService.Domain.Entities;
using CarBookingService.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;
using CarBookingService.Application.Services;

namespace CarBookingService.Infrastructure.Messaging
{
    public class RabbitMQConsumer : IRabbitMQConsumer
    {
        private readonly IServiceProvider _serviceProvider;
        private IConnection? _connection;
        private IModel? _channel;

        public RabbitMQConsumer(IServiceProvider serviceProvider, IConfiguration config)
        {
            _serviceProvider = serviceProvider;
            InitializeRabbitMQ(config);
        }

        private void InitializeRabbitMQ(IConfiguration config)
        {
            var factory = new ConnectionFactory()
            {
                HostName = config["RabbitMQ:Host"],
                UserName = config["RabbitMQ:Username"],
                Password = config["RabbitMQ:Password"]
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "bookingValidationQueue", durable: true, exclusive: false, autoDelete: false);
        }

        public void StartConsuming()
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, eventArgs) =>
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var bookingService = scope.ServiceProvider.GetRequiredService<IBookingService>();

                    var body = eventArgs.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var booking = JsonConvert.DeserializeObject<CarBooking>(message);

                    if (booking != null)
                    {
                        await bookingService.UpdateBookingStatusAsync(booking);
                    }
                }

                _channel.BasicAck(eventArgs.DeliveryTag, false);
            };

            _channel.BasicConsume(queue: "bookingValidationQueue", autoAck: false, consumer: consumer);
        }
    }
}
