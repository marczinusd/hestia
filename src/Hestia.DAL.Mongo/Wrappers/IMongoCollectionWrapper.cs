using System;
using System.Collections.Generic;
using MongoDB.Driver.Linq;

namespace Hestia.DAL.Mongo.Wrappers
{
    public interface IMongoCollectionWrapper<T>
    {
        IEnumerable<T> Find(Func<T, bool> entity);

        void InsertOne(T entity);

        IMongoQueryable<T> AsQueryable();
    }
}
