using MongoDB.Driver;

namespace Hestia.DAL.Mongo
{
    public interface IMongoClientFactory
    {
        IMongoClient CreateClient(string connectionString);
    }
}
