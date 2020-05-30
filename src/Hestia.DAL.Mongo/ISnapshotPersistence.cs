using Hestia.Model;

namespace Hestia.DAL.Mongo
{
    public interface ISnapshotPersistence
    {
        void InsertSnapshot(RepositorySnapshot snapshot);
    }
}
