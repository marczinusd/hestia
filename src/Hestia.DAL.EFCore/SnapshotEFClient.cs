using System;
using System.Collections.Generic;
using Hestia.DAL.Interfaces;
using Hestia.Model.Interfaces;
using LanguageExt;

namespace Hestia.DAL.EFCore
{
    public class SnapshotEFClient : ISnapshotRetrieval, ISnapshotPersistence
    {
        public IEnumerable<ISnapshotHeader> GetAllSnapshotsHeaders()
        {
            throw new NotImplementedException();
        }

        public IRepositorySnapshotEntity GetSnapshotById(string id)
        {
            throw new NotImplementedException();
        }

        public IObservable<Unit> InsertSnapshot(IRepositorySnapshot snapshot)
        {
            throw new NotImplementedException();
        }
    }
}
