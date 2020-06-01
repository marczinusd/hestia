using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using Hestia.DAL.Mongo.Model;
using Hestia.DAL.Mongo.Wrappers;
using Hestia.Model;
using Hestia.Model.Extensions;
using MongoDB.Driver;

namespace Hestia.DAL.Mongo
{
    public class SnapshotMongoClient : ISnapshotPersistence
    {
        private const string CollectionName = "snapshots";
        private readonly IMongoCollectionWrapper<RepositorySnapshotEntity> _snapshots;

        public SnapshotMongoClient(IMongoClientFactory factory,
                                   Func<IMongoCollection<RepositorySnapshotEntity>,
                                       IMongoCollectionWrapper<RepositorySnapshotEntity>> wrapperFactory,
                                   string databaseName)
        {
            var client = factory.CreateClient();
            var database = client.GetDatabase(databaseName, new MongoDatabaseSettings());

            _snapshots = wrapperFactory(database.GetCollection<RepositorySnapshotEntity>(CollectionName));
        }

        public IObservable<IEnumerable<RepositorySnapshotEntity>> GetAllSnapshots() =>
            ObservableExt.CreateSingle(() => _snapshots.Find(r => true));

        public IObservable<RepositorySnapshotEntity> GetSnapshotById(string id) =>
            ObservableExt.CreateSingle(() => _snapshots.Find(r => r.Id.ToString()!.Equals(id, StringComparison.InvariantCulture))
                                                       .FirstOrDefault());

        public IObservable<Unit> InsertSnapshot(RepositorySnapshot snapshot) =>
            ObservableExt.CreateSingle(() =>
            {
                _snapshots.InsertOne(new RepositorySnapshotEntity(snapshot));
                return Unit.Default;
            });
    }
}
