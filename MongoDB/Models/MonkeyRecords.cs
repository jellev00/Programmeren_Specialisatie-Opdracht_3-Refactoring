using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscapeFromTheWoods.MongoDB.Models
{
    public class MonkeyRecords
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string RecordID { get; set; }

        public int MonkeyID { get; set; }
        public string MonkeyName { get; set; }
        public int WoodID { get; set; }
        public int Seqnr { get; set; }
        public int TreeID { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}
