using System.Collections.Generic;
using Hestia.Model.Interfaces;

namespace Hestia.DAL.Interfaces
{
    public interface ISnapshotRetrieval
    {
        IEnumerable<ISnapshotHeader> GetAllSnapshotsHeaders();

        IRepositorySnapshotEntity GetSnapshotById(string id);
    }
}
