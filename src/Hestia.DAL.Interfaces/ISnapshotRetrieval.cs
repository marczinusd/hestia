using System.Collections.Generic;
using Hestia.Model.Interfaces;
using LanguageExt;

namespace Hestia.DAL.Interfaces
{
    public interface ISnapshotRetrieval
    {
        IEnumerable<ISnapshotHeader> GetAllSnapshotsHeaders();

        Option<IRepositorySnapshotEntity> GetSnapshotById(string id);
    }
}
