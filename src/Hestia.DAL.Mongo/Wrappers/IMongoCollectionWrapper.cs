using System;
using System.Collections.Generic;

namespace Hestia.DAL.Mongo.Wrappers
{
    public interface IMongoCollectionWrapper<T>
    {
        IEnumerable<T> Find(Func<T, bool> entity);

        void InsertOne(T entity);
    }
}
