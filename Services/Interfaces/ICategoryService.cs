using Beauty.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Beauty.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<List<Category>> Get();
        Task<Category> Get(string id);
        Task Create(Category Business);
        Task Update(string id, Category BusinessIn);
        Task Remove(Category BusinessIn);
        Task Remove(string id);
    }
}