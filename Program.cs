
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using CarBookingService.Infrastructure.Messaging;
using CarBookingService.Infrastructure.Persistence;
using CarBookingService.Application.Interfaces;
using CarBookingService.Application.Services;

namespace OrderService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            // Add MongoDB
            builder.Services.AddSingleton<IMongoClient>(sp => new MongoClient(builder.Configuration["MongoDB:ConnectionString"]));
            builder.Services.AddScoped<IMongoDatabase>(sp => sp.GetRequiredService<IMongoClient>().GetDatabase("car-rental-db"));

            // Add Dependencies
            builder.Services.AddScoped<ICarBookingRepository, CarBookingRepository>();
            builder.Services.AddScoped<IBookingService, BookingService>();
            builder.Services.AddSingleton<IRabbitMQProducer, RabbitMQProducer>();
            builder.Services.AddSingleton<IRabbitMQConsumer, RabbitMQConsumer>();



            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Order Service API",
                    Version = "v1",
                    Description = "Handles order creation and management"
                });
                c.EnableAnnotations();
            });

            // Start RabbitMQ Consumer
            var app = builder.Build();
            var rabbitMQConsumer = app.Services.GetRequiredService<IRabbitMQConsumer>();
            rabbitMQConsumer.StartConsuming();


            // Enable Swagger UI
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "OrderService API V1");
                    c.RoutePrefix = string.Empty;
                });
            }

            app.UseRouting();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
