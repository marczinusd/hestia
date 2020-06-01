using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Hestia.Model;
using JetBrains.Annotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Hestia.DAL.Mongo.Model
{
    [ExcludeFromCodeCoverage]
    public class RepositorySnapshotEntity
    {
        private readonly RepositorySnapshot _snapshot;

        public RepositorySnapshotEntity(RepositorySnapshot snapshot)
        {
            _snapshot = snapshot;
        }

        [BsonId] [UsedImplicitly] public ObjectId? Id { get; } = ObjectId.GenerateNewId();

        [UsedImplicitly]
        [BsonElement]
        public IEnumerable<FileEntity> Files => _snapshot.Files
                                                         .Select(f => new FileEntity(f));

        [UsedImplicitly]
        [BsonElement]
        public string AtHash => _snapshot.AtHash
                                         .Match(x => x, string.Empty);
    }
}
