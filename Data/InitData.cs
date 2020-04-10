using Beauty.Models;
using Beauty.Services.Interfaces;
using Beauty.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace Beauty.Data
{
    public class InitData
    {
        private readonly IMongoCollection<Business> businesses;
        private readonly IWebHostEnvironment environment;
        private readonly IMongoCollection<Category> categories;

        public InitData(MongoDatabase db, IWebHostEnvironment env)
        {
            businesses = db.GetCollection<Business>();
            categories = db.GetCollection<Category>();
            environment = env;
        }
        public void GenerateEssentialData()
        {

        }
        public void GenerateTestData()
        {
            GenerateCategories();
            GenerateBusinesses();
        }
        private void GenerateCategories()
        {
            if (categories.CountDocuments(ct => true) == 0)
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                };

                var jsonString = File.ReadAllText(Path.Combine(environment.ContentRootPath, "Data", "Test", "Categories.json"));
                var jsonModel = JsonSerializer.Deserialize<List<Category>>(jsonString, options);
                categories.InsertMany(jsonModel);
            }

        }
        private void GenerateBusinesses()
        {
            if (businesses.CountDocuments(ct => true) == 0)
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    IgnoreNullValues=true,
                    
                };
                
                var jsonString = File.ReadAllText(Path.Combine(environment.ContentRootPath, "Data", "Test", "Businesses.json"));
                var jsonModel = JsonSerializer.Deserialize<List<Business>>(jsonString, options);
                businesses.InsertMany(jsonModel);
            }

        }
    }
}
