using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Pluralize.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Beauty.Data
{
    public class MongoDatabase
    {
        IMongoDatabase db;
        public MongoDatabase(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("BeautyDb"));
            db = client.GetDatabase("Beauty");
        }
        public IMongoCollection<T> GetCollection<T>()
        {

            IPluralize pluralizer = new Pluralizer();
            return db.GetCollection<T>(pluralizer.Pluralize(typeof(T).Name));
        }

    }
}
