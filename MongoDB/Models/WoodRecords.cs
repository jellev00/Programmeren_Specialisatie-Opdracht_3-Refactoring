using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscapeFromTheWoods.MongoDB.Models
{
    public class WoodRecords
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string RecordID { get; set; }

        public int WoodID { get; set; }
        public int TreeID { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}
