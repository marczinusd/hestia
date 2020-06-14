using System;
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
    [BsonIgnoreExtraElements]
    public class RepositorySnapshotEntity
    {
        public RepositorySnapshotEntity(RepositorySnapshot snapshot)
        {
            Id = ObjectId.GenerateNewId();
            Files = snapshot.Files.Select(f => new FileEntity(f));
            AtHash = snapshot.AtHash.Match(x => x, string.Empty);
            HashDate = snapshot.CommitCreationDate.Match(x => x, null);
        }

        [BsonConstructor]
        public RepositorySnapshotEntity(ObjectId? id, IEnumerable<FileEntity> files, string atHash, DateTime? hashDate)
        {
            Id = id;
            Files = files;
            AtHash = atHash;
            HashDate = hashDate;
        }

        [BsonId] [UsedImplicitly] public ObjectId? Id { get; }

        [UsedImplicitly] [BsonElement] public IEnumerable<FileEntity> Files { get; }

        [UsedImplicitly] [BsonElement] public string AtHash { get; }

        [UsedImplicitly] [BsonElement] public DateTime? HashDate { get; }
    }
}
