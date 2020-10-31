using System;
using Hestia.Model.Interfaces;
using LanguageExt;

namespace Hestia.DAL.Interfaces
{
    public interface ISnapshotPersistence
    {
        IObservable<int> InsertSnapshot(IRepositorySnapshot snapshot);

        int InsertSnapshotSync(IRepositorySnapshot snapshot);
    }
}
