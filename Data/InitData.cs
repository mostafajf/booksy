using Beauty.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Beauty.Data
{
    public class InitData
    {
        public static void InitializeData(MongoDatabase db)
        {
            GenerateEssentialData(db);
            GenerateTestData(db);
        }
        static void GenerateEssentialData(MongoDatabase db)
        {

        }
        static void GenerateTestData(MongoDatabase db)
        {
            List<Business> bussinesses = new List<Business>
            {
                new Business
                {
                    
                }
            };
        }
    }
}
