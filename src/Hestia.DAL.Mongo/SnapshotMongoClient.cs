using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using Hestia.DAL.Mongo.Model;
using Hestia.DAL.Mongo.Wrappers;
using Hestia.Model;
using Hestia.Model.Extensions;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Hestia.DAL.Mongo
{
    public class SnapshotMongoClient : ISnapshotPersistence, ISnapshotRetrieval
    {
        private const string CollectionName = "snapshots";
        private readonly IMongoQueryable<RepositorySnapshotEntity> _queryable;
        private readonly IMongoCollectionWrapper<RepositorySnapshotEntity> _mongoCollection;

        public SnapshotMongoClient(IMongoClientFactory factory,
                                   Func<IMongoCollection<RepositorySnapshotEntity>,
                                       IMongoCollectionWrapper<RepositorySnapshotEntity>> wrapperFactory,
                                   string databaseName)
        {
            var client = factory.CreateClient();
            var database = client.GetDatabase(databaseName, new MongoDatabaseSettings());

            _mongoCollection = wrapperFactory(database.GetCollection<RepositorySnapshotEntity>(CollectionName));
            _queryable = _mongoCollection.AsQueryable();
        }

        public IEnumerable<SnapshotHeader> GetAllSnapshotsHeaders() =>
            _queryable
                .ToList()
                .Select(snapshot =>
                            new SnapshotHeader(string.Empty,
                                               string.Empty,
                                               snapshot.AtHash,
                                               snapshot.HashDate ?? DateTime.MinValue));

        public RepositorySnapshotEntity GetSnapshotById(string id) =>
            _queryable
                .FirstOrDefault(r => r.Id.ToString()!.Equals(id, StringComparison.InvariantCulture));

        public IObservable<Unit> InsertSnapshot(RepositorySnapshot snapshot) =>
            ObservableExt.CreateSingle(() =>
            {
                _mongoCollection.InsertOne(new RepositorySnapshotEntity(snapshot));
                return Unit.Default;
            });
    }
}
