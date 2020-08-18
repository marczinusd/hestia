using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Hestia.DAL.EFCore.Adapters;
using Hestia.DAL.EFCore.Entities;
using Hestia.DAL.Interfaces;
using Hestia.Model.Interfaces;
using LanguageExt;
using static LanguageExt.Prelude;

namespace Hestia.DAL.EFCore
{
    public class SnapshotEFClient : ISnapshotRetrieval, ISnapshotPersistence, IFileRetrieval, IDisposable
    {
        private readonly HestiaContext _dbContext;

        public SnapshotEFClient(HestiaContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Dispose() => _dbContext?.Dispose();

        public Option<IFileEntity> GetFileDetails(string fileId, string snapshotId) =>
            _dbContext.Files.FirstOrDefault(f => f.Id == fileId && f.Parent.Id == snapshotId) is { } result
                ? Some<IFileEntity>(new FileEntityAdapter(result))
                : None;

        public IObservable<Unit> InsertSnapshot(IRepositorySnapshot snapshot)
        {
            _dbContext.Snapshots.Add(snapshot.AsEntity());

            return Observable.FromAsync(_dbContext.SaveChangesAsync)
                             .Select(x => Unit.Default);
        }

        public IEnumerable<ISnapshotHeader> GetAllSnapshotsHeaders() => _dbContext.Snapshots
            .Select(s => new RepositorySnapshotEntityAdapter(s))
            .ToList();

        public Option<IRepositorySnapshotEntity> GetSnapshotById(string id) =>
            _dbContext.Snapshots.SingleOrDefault(s => s.Id == id) is { } entity
                ? Some<IRepositorySnapshotEntity>(new RepositorySnapshotEntityAdapter(entity))
                : None;
    }
}
