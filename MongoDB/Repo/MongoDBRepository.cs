using EscapeFromTheWoods.MongoDB.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscapeFromTheWoods.MongoDB.Repo
{
    public class MongoDBRepository
    {
        private IMongoDatabase _database;
        private IMongoClient _client;
        private string _connectionString;

        public MongoDBRepository(string connectionString)
        {
            _connectionString = connectionString;
            _client = new MongoClient(connectionString);
            _database = _client.GetDatabase("refactoring_DB");
        }

        // Insert WoodRecord
        public async Task InsertWoodRecordAsync(List<WoodRecords> woodRecord)
        {
            var woodRecordsCollection = _database.GetCollection<WoodRecords>("WoodRecords");
            await woodRecordsCollection.InsertManyAsync(woodRecord);
        }

        // Insert MonkeyRecord
        public async Task InsertMonkeyRecordAsync(List<MonkeyRecords> monkeyRecord)
        {
            var monkeyRecordsCollection = _database.GetCollection<MonkeyRecords>("MonkeyRecords");
            await monkeyRecordsCollection.InsertManyAsync(monkeyRecord);
        }

        // Insert Log
        public async Task InsertLogAsync(List<Logs> logs)
        {
            var logsCollection = _database.GetCollection<Logs>("Logs");
            await logsCollection.InsertManyAsync(logs);
        }
    }
}
