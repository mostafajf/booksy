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
    public class CategoryService : ICategoryService
    {
        private readonly IMongoCollection<Category> Categories;

        public CategoryService(IConfiguration config, MongoDatabase db)
        {

            Categories = db.GetCollection<Category>();
        }

        public async Task<List<Category>> Get()
        {
            return (await Categories.FindAsync(Business => true)).ToList();
        }

        public async Task<Category> Get(string id)
        {
            return (await Categories.FindAsync<Category>(Category => Category.Id == id)).FirstOrDefault();
        }

        public async Task Create(Category Category)
        {
            await Categories.InsertOneAsync(Category);
        }

        public async Task Update(string id, Category BusinessIn)
        {
            await Categories.ReplaceOneAsync(Category => Category.Id == id, BusinessIn);
        }

        public async Task Remove(Category BusinessIn)
        {
            await Categories.DeleteOneAsync(Category => Category.Id == BusinessIn.Id);
        }

        public async Task Remove(string id)
        {
            await Categories.DeleteOneAsync(Category => Category.Id == id);
        }

    }
}
