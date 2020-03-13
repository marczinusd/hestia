using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Hestia.DAL.Mongo.Model;
using Hestia.DAL.Mongo.Wrappers;
using Hestia.Model;
using Hestia.Model.Extensions;
using MongoDB.Driver;

namespace Hestia.DAL.Mongo
{
    public class HestiaMongoClient
    {
        private readonly IMongoCollectionWrapper<RepositoryEntity> _repositories;

        public HestiaMongoClient(IMongoClientFactory factory,
                                 Func<IMongoCollection<RepositoryEntity>,
                                     IMongoCollectionWrapper<RepositoryEntity>> wrapperFactory,
                                 string connectionString,
                                 string databaseName,
                                 string collectionName)
        {
            var client = factory.CreateClient(connectionString);
            var database = client.GetDatabase(databaseName, new MongoDatabaseSettings());

            _repositories = wrapperFactory(database.GetCollection<RepositoryEntity>(collectionName));
        }

        public IObservable<IEnumerable<RepositoryEntity>> GetAllRepos() =>
            ObservableExt.CreateSingle(() => _repositories.Find(r => true));

        public IObservable<RepositoryEntity> GetRepoById(string id) =>
            ObservableExt.CreateSingle(() => _repositories.Find(r => r.Id == id)
                                                          .FirstOrDefault());

        public void AddRepository(Repository repository) =>
            _repositories.InsertOne(new RepositoryEntity(repository));

        // public RepositorySnapshot GetSnapshotFromRepository(string repositoryId, string snapshotId) =>
        //     _repositories.Find(r => r.Id == repositoryId)
        //                  .FirstOrDefault()
        //                  .Snapshots
        //                  .Match(v => v,
        //                         () => null)
        //                  .FirstOrDefault(s => s.SnapshotId.ToString() == snapshotId);
    }
}
