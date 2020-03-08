using MongoDB.Driver;

namespace Hestia.DAL.Mongo
{
    public class MongoClientFactory : IMongoClientFactory
    {
        public IMongoClient CreateClient(string connectionString)
            => new MongoClient(connectionString);
    }
}
