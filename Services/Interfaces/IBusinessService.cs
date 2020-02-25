using Beauty.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Beauty.Services.Interfaces
{
    interface IBusinessService
    {
        Task<List<Business>> Get();
        Task<Business> Get(string id);
        Task Create(Business Business);
        Task Update(string id, Business BusinessIn);
        Task Remove(Business BusinessIn);
        Task Remove(string id);
        Task<List<Business>> GetNearby(long latitude, long longitude);
    }
}