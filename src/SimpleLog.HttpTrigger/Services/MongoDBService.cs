using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using SimpleLog.HttpTrigger.Entities;
using SimpleLog.HttpTrigger.Services.Interfaces;

namespace SimpleLog.HttpTrigger.Services
{
    public class MongoDBService : IDatabaseService
    {
        private readonly string _connectionString;

        public MongoDBService(string connectionString)
        {
            this._connectionString = connectionString;
        }

        public async Task<bool> SaveAsync(ILogger log, SimpleLogEntity entity)
        {
            try
            {      
                var database = this.GetMongoDatabase();
                var collection = database
                    .GetCollection<SimpleLogEntity>(typeof(SimpleLogEntity).Name);
                
                await collection.InsertOneAsync(entity);
                return true;
            }
            catch (Exception ex)
            {
                log.LogError(ex, ex.Message);
                return false;
            }
        }

        private IMongoDatabase GetMongoDatabase()
        {
            var mongoUrl = MongoUrl.Create(this._connectionString);
            var settings = MongoClientSettings.FromUrl(mongoUrl);
            var client = new MongoClient(settings);
            return client.GetDatabase(mongoUrl.DatabaseName);
        }
    }
}
