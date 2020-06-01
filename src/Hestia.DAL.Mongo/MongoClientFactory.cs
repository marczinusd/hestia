using System;
using System.Diagnostics.CodeAnalysis;
using MongoDB.Driver;

namespace Hestia.DAL.Mongo
{
    [ExcludeFromCodeCoverage]
    public class MongoClientFactory : IMongoClientFactory
    {
        public static string DatabaseName => Environment.GetEnvironmentVariable("HESTIA_MONGO_DB") ?? "hestia";

        public IMongoClient CreateClient()
        {
            return new MongoClient(CreateConnectionString());
        }

        private static string CreateConnectionString()
        {
            var host = Environment.GetEnvironmentVariable("HESTIA_MONGO_HOST") ?? "localhost";
            var port = Environment.GetEnvironmentVariable("HESTIA_MONGO_PORT") ?? "27017";

            return $"mongodb://{CreateAuthConnectionString()}{host}:{port}";
        }

        private static string CreateAuthConnectionString()
        {
            var username = Environment.GetEnvironmentVariable("HESTIA_MONGO_USER");
            var password = Environment.GetEnvironmentVariable("HESTIA_MONGO_PWD");

            return !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password)
                       ? $"{username}:{password}@"
                       : string.Empty;
        }
    }
}
