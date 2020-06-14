using System.Collections.Generic;
using Hestia.DAL.Mongo.Model;
using Hestia.Model;

namespace Hestia.DAL.Mongo
{
    public interface ISnapshotRetrieval
    {
        IEnumerable<SnapshotHeader> GetAllSnapshotsHeaders();

        RepositorySnapshotEntity GetSnapshotById(string id);
    }
}
