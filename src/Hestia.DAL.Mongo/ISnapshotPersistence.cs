using System;
using System.Reactive;
using Hestia.Model;

namespace Hestia.DAL.Mongo
{
    public interface ISnapshotPersistence
    {
        IObservable<Unit> InsertSnapshot(RepositorySnapshot snapshot);
    }
}
