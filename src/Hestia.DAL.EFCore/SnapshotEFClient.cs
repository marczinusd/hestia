using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Hestia.DAL.EFCore.Entities;
using Hestia.DAL.Interfaces;
using Hestia.Model.Interfaces;
using LanguageExt;
using static LanguageExt.Prelude;

namespace Hestia.DAL.EFCore
{
    public class SnapshotEFClient : ISnapshotRetrieval, ISnapshotPersistence, IFileRetrieval
    {
        public IEnumerable<ISnapshotHeader> GetAllSnapshotsHeaders()
        {
            using var context = new HestiaContext();
            return context.Snapshots.ToList();
        }

        public Option<IRepositorySnapshotEntity> GetSnapshotById(string id)
        {
            using var context = new HestiaContext();
            var entity = context.Snapshots.SingleOrDefault(s => s.Id == id);

            return entity != null ? Some(entity) : Option<IRepositorySnapshotEntity>.None;
        }

        public IObservable<Unit> InsertSnapshot(IRepositorySnapshot snapshot)
        {
            using var context = new HestiaContext();
            context.Snapshots.Add(snapshot.AsEntity());

            return Observable.FromAsync(context.SaveChangesAsync)
                             .Select(x => Unit.Default);
        }

        public Option<IFileEntity> GetFileDetails(string fileId, string snapshotId)
        {
            using var context = new HestiaContext();
            var file = context.Snapshots.SingleOrDefault(s => s.Id == snapshotId)
                              ?.Files.SingleOrDefault(f => f.Id == fileId);

            return file != null ? Some(file) : Option<IFileEntity>.None;
        }
    }
}
