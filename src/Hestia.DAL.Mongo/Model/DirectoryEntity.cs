using System.Text.Json.Serialization;
using Hestia.Model;
using MongoDB.Bson.Serialization.Attributes;

namespace Hestia.DAL.Mongo.Model
{
    public class DirectoryEntity
    {
        private readonly Directory _directory;

        public DirectoryEntity(Directory directory, string? id = null)
        {
            _directory = directory;
            Id = id;
        }

        [BsonId]
        public string? Id { get; }

        [JsonIgnore]
        public Directory Directory => _directory;
    }
}
