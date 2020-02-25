using Beauty.Data;
using Beauty.Models;
using Beauty.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Beauty.Services
{
    public class BusinessService : IBusinessService
    {
        private readonly IMongoCollection<Business> businesses;

        public BusinessService(IConfiguration config, MongoDatabase db)
        {

            businesses = db.GetCollection<Business>();
        }

        public async Task<List<Business>> Get()
        {
            return (await businesses.FindAsync(Business => true)).ToList();
        }

        public async Task<Business> Get(string id)
        {
            return (await businesses.FindAsync<Business>(Business => Business.Id == id)).FirstOrDefault();
        }

        public async Task Create(Business Business)
        {
            await businesses.InsertOneAsync(Business);
        }

        public async Task Update(string id, Business BusinessIn)
        {
            await businesses.ReplaceOneAsync(Business => Business.Id == id, BusinessIn);
        }

        public async Task Remove(Business BusinessIn)
        {
            await businesses.DeleteOneAsync(Business => Business.Id == BusinessIn.Id);
        }

        public async Task Remove(string id)
        {
            await businesses.DeleteOneAsync(Business => Business.Id == id);
        }

        public async Task<List<Business>> GetNearby(long latitude, long longitude)
        {
            var point = GeoJson.Point(GeoJson.Geographic(latitude, longitude)); //long, lat
            var builder = Builders<Business>.Filter;
            var filter = builder.Near(bs => bs.Location, point, minDistance: 10);
            return (await businesses.FindAsync(filter)).ToList();

        }
    }
}
