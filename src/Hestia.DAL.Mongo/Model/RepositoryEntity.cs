using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Hestia.Model;
using MongoDB.Bson.Serialization.Attributes;

namespace Hestia.DAL.Mongo.Model
{
    [ExcludeFromCodeCoverage]
    public class RepositoryEntity
    {
        private readonly Repository _repository;

        public RepositoryEntity(Repository repository, string? id)
        {
            _repository = repository;
            Id = id;
        }

        [BsonId]
        public string? Id { get; }

        [JsonIgnore]
        public Repository Repository => _repository;
    }
}
