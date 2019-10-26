using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Beauty.Models
{
    public class Category : EntityBase
    {
        [BsonElement("Name")]
        public string Name { get; set; }
    }
}
