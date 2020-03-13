using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Hestia.Model;
using MongoDB.Bson.Serialization.Attributes;

namespace Hestia.DAL.Mongo.Model
{
    [ExcludeFromCodeCoverage]
    public class RepositorySnapshotEntity
    {
        private readonly RepositorySnapshot _snapshot;

        public RepositorySnapshotEntity(RepositorySnapshot snapshot, string? id = null)
        {
            _snapshot = snapshot;
            Id = id;
        }

        [BsonId]
        public string? Id { get; }

        [JsonIgnore]
        public RepositorySnapshot Snapshot => _snapshot;
    }
}
