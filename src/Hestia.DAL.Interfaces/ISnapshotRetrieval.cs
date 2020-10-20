using System.Collections.Generic;
using Hestia.Model.Interfaces;
using LanguageExt;

namespace Hestia.DAL.Interfaces
{
    public interface ISnapshotRetrieval
    {
        IEnumerable<ISnapshotHeader> GetAllSnapshotsHeaders();

        Option<IRepositorySnapshotEntity> GetSnapshotById(string id);
        IEnumerable<IFileEntity> GetAllFilesForSnapshot(string snapshotId);
        IEnumerable<ILineEntity> GetFileContent(string fileId);
        bool SnapshotExistsWithId(string snapshotId);
        bool FileExistsWithId(string fileId);
    }
}
