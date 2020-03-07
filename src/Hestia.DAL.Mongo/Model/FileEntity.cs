using System.Text.Json.Serialization;
using Hestia.Model;
using MongoDB.Bson.Serialization.Attributes;

namespace Hestia.DAL.Mongo.Model
{
    public class FileEntity
    {
        private readonly File _file;

        public FileEntity(File file, string? id = null)
        {
            _file = file;
            Id = id;
        }

        [BsonId]
        public string? Id { get; }

        [JsonIgnore]
        public File File => _file;
    }
}
