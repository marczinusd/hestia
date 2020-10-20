using Hestia.DAL.EFCore.Entities;
using Hestia.DAL.Interfaces;

namespace Hestia.DAL.EFCore.Adapters
{
    public static class AdapterExtensions
    {
        public static IFileEntity AsModel(this File entity) => new FileEntityAdapter(entity);

        public static IRepositorySnapshotEntity AsModel(this Snapshot entity) =>
            new RepositorySnapshotEntityAdapter(entity);

        public static ILineEntity AsModel(this Line entity) => new LineEntityAdapter(entity);
    }
}
