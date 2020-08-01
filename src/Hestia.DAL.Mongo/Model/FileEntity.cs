using System.Diagnostics.CodeAnalysis;
using Hestia.Model;
using JetBrains.Annotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Hestia.DAL.Mongo.Model
{
    [ExcludeFromCodeCoverage]
    [BsonIgnoreExtraElements]
    public class FileEntity : IFileEntity
    {
        public FileEntity(File file)
        {
            Id = ObjectId.GenerateNewId();
            Path = file.Path;
            LifetimeChanges = file.LifetimeChanges;
            LifetimeAuthors = file.LifetimeAuthors;
            CoveragePercentage = file.CoveragePercentage;
        }

        [BsonConstructor]
        public FileEntity(ObjectId? id,
                          string path,
                          int lifetimeChanges,
                          int lifetimeAuthors,
                          decimal coveragePercentage)
        {
            Id = id;
            Path = path;
            LifetimeChanges = lifetimeChanges;
            LifetimeAuthors = lifetimeAuthors;
            CoveragePercentage = coveragePercentage;
        }

        [BsonId] [UsedImplicitly] public ObjectId? Id { get; }

        [UsedImplicitly] [BsonElement] public string Path { get; }

        [UsedImplicitly] [BsonElement] public int LifetimeChanges { get; }

        [UsedImplicitly] [BsonElement] public int LifetimeAuthors { get; }

        [UsedImplicitly] [BsonElement] public decimal CoveragePercentage { get; }
    }
}
