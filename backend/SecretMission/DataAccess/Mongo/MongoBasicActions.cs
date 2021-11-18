using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using SecretMission.Helpers;

namespace SecretMission.DB
{
    public class MongoBasicActions
    {
        private static MongoBasicActions MongoInstance = null;
        private static MongoClient Client = null;
        private static IMongoDatabase DB = null;


        private MongoBasicActions() { }

        public static MongoBasicActions Instance()
        {
            if (MongoInstance == null)
            {
                var connectionString = GetConnectionString();
                MongoInstance = new MongoBasicActions();
                Client = new MongoClient(connectionString);
                DB = Client.GetDatabase(Constants.DB_NAME);
            }
            return MongoInstance;
        }

        public async Task<List<T>> GetAllObjectsAsync<T>()
        {
            var collection = DB.GetCollection<T>(Constants.COLLECTION_NAME);
            var filter = Builders<T>.Filter.Empty;

            return (await collection.FindAsync(filter)).ToList();
        }

        private static string GetConnectionString()
        {
            var envData = System.Environment.GetEnvironmentVariable("IsDocker");
            if (envData != null && envData.ToLower() == "true")
            {
                return Constants.DOCKER_DB_CONNECTION_STRING;
            }
            return Constants.LOCAL_DB_CONNECTION_STRING;
        }

        internal async Task AddNewObjectAsync<T>(T newObject)
        {
            var collection = DB.GetCollection<T>(Constants.COLLECTION_NAME);
            await collection.InsertOneAsync(newObject);
        }
    }
}

