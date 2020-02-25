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

namespace Beauty.Data
{
    public class InitData
    {
        private readonly IMongoCollection<Business> businesses;
        private readonly IWebHostEnvironment environment;
        private readonly IMongoCollection<Category> categories;
        private readonly ICategoryService categoryService;
        private readonly IBusinessService bussinessService;


        InitData(MongoDatabase db, IWebHostEnvironment environment, ICategoryService cs, IBusinessService bs)
        {
            businesses = db.GetCollection<Business>();
            categories = db.GetCollection<Category>();
            categoryService = cs;
            bussinessService = bs;
        }
        public void GenerateEssentialData()
        {

        }
        public void GenerateTestData()
        {
            GenerateCategories();

        }
        private void GenerateCategories()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            var jsonString = File.ReadAllText(Path.Combine(environment.ContentRootPath, "Data", "Test", "Categories.json"));
            var jsonModel = JsonSerializer.Deserialize<List<Category>>(jsonString, options);
        }
    }
}
