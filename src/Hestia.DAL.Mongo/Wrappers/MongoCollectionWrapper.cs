using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using MongoDB.Driver;

namespace Hestia.DAL.Mongo.Wrappers
{
    [ExcludeFromCodeCoverage]
    public class MongoCollectionWrapper<T> : IMongoCollectionWrapper<T>
    {
        private readonly IMongoCollection<T> _collection;

        public MongoCollectionWrapper(IMongoCollection<T> collection)
        {
            _collection = collection;
        }

        public IEnumerable<T> Find(Func<T, bool> entity) =>
            _collection.Find(e => entity(e))
                       .ToList();

        public void InsertOne(T entity) => _collection.InsertOne(entity);
    }
}
