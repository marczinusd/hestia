using System;
using Hestia.Model.Interfaces;
using LanguageExt;

namespace Hestia.DAL.Interfaces
{
    public interface ISnapshotPersistence
    {
        IObservable<Unit> InsertSnapshot(IRepositorySnapshot snapshot);
    }
}
