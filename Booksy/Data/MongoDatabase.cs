using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Booksy.Data
{
    public class MongoDatabase
    {
        IMongoDatabase db;
        public MongoDatabase(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("BooksyDb"));
            db = client.GetDatabase("booksy");
        }
        public IMongoCollection<T> GetCollection<T>()
        {
            return db.GetCollection<T>(nameof(T));
        }

    }
}
