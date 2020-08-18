using Hestia.DAL.EFCore.Entities;
using Hestia.DAL.Interfaces;

namespace Hestia.DAL.EFCore.Adapters
{
    public static class AdapterExtensions
    {
        public static IFileEntity AsModel(this FileEntity entity) => new FileEntityAdapter(entity);

        public static IRepositorySnapshotEntity AsModel(this RepositorySnapshotEntity entity) =>
            new RepositorySnapshotEntityAdapter(entity);

        public static ILineEntity AsModel(this LineEntity entity) => new LineEntityAdapter(entity);
    }
}
