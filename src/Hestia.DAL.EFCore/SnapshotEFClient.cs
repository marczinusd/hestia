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

        public Option<IFileEntity> GetFileDetails(string fileId) =>
            _dbContext.Files.FirstOrDefault(f => f.Id == fileId) is { } result
                ? Some<IFileEntity>(new FileEntityAdapter(result))
                : None;

        public IEnumerable<ILineEntity> GetLinesForFile(string fileId) =>
            _dbContext.SourceLines
                      .Where(l => l.FileId == fileId)
                      .Select(l => new LineEntityAdapter(l));

        public IObservable<Unit> InsertSnapshot(IRepositorySnapshot snapshot)
        {
            _dbContext.Snapshots.Add(snapshot.AsEntity());

            return Observable.FromAsync(_dbContext.SaveChangesAsync)
                             .Select(x => Unit.Default);
        }

        public IEnumerable<ISnapshotHeader> GetAllSnapshotsHeaders() => _dbContext.Snapshots
            .Select(s => new RepositorySnapshotEntityAdapter(s))
            .ToList();

        public IEnumerable<IFileEntity> GetAllFilesForSnapshot(string snapshotId) =>
            _dbContext.Files
                      .Where(f => f.SnapshotId == snapshotId)
                      .Select(f => new FileEntityAdapter(f))
                      .ToList();

        public bool FileExistsWithId(string fileId) => _dbContext.Files.Any(f => f.Id == fileId);

        public Option<IRepositorySnapshotEntity> GetSnapshotById(string id) =>
            _dbContext.Snapshots.SingleOrDefault(s => s.Id == id) is { } entity
                ? Some<IRepositorySnapshotEntity>(new RepositorySnapshotEntityAdapter(entity))
                : None;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext?.Dispose();
            }
        }
    }
}
