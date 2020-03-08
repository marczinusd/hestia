using System.Diagnostics.CodeAnalysis;
using MongoDB.Driver;

namespace Hestia.DAL.Mongo
{
    [ExcludeFromCodeCoverage]
    public class MongoClientFactory : IMongoClientFactory
    {
        public IMongoClient CreateClient(string connectionString)
            => new MongoClient(connectionString);
    }
}
