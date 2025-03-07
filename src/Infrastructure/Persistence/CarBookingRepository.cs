using CarBookingService.Domain.Entities;
using CarBookingService.Application.Interfaces;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace CarBookingService.Infrastructure.Persistence
{
    public class CarBookingRepository : ICarBookingRepository
    {
        private readonly IMongoCollection<CarBooking> _collection;

        public CarBookingRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<CarBooking>("car_orders");
        }

        public async Task InsertBookingAsync(CarBooking booking)
        {
            await _collection.InsertOneAsync(booking);
        }
        public async Task<CarBooking?> GetBookingByIdAsync(string id)
        {
            var res = await _collection.Find(b => b.Id == id).FirstOrDefaultAsync();
            return res;
        }
        public async Task UpdateBookingStatusAsync(string id, string status)
        { 
            await _collection.UpdateOneAsync(b => b.Id == id, Builders<CarBooking>.Update.Set(b => b.Status, status));
        }
    }
}
