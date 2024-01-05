using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscapeFromTheWoods.MongoDB.Models
{
    public class Logs
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ID { get; set; }

        public int WoodID { get; set; }
        public int MonkeyID { get; set; }
        public string Message { get; set; }
    }
}
