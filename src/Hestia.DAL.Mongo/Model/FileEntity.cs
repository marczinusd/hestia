using System.Diagnostics.CodeAnalysis;
using Hestia.Model;
using JetBrains.Annotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Hestia.DAL.Mongo.Model
{
    [ExcludeFromCodeCoverage]
    public class FileEntity
    {
        private readonly File _file;

        public FileEntity(File file)
        {
            _file = file;
        }

        [BsonId] [UsedImplicitly] public ObjectId? Id { get; } = ObjectId.GenerateNewId();

        [UsedImplicitly] [BsonElement] public string Path => _file.FullPath;

        [UsedImplicitly] [BsonElement] public int LifetimeChanges => _file.LifetimeChanges;

        [UsedImplicitly] [BsonElement] public int LifetimeAuthors => _file.LifetimeAuthors;

        [UsedImplicitly] [BsonElement] public decimal CoveragePercentage => _file.CoveragePercentage;
    }
}
