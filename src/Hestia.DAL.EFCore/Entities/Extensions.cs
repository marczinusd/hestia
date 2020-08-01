using System.Linq;
using Hestia.DAL.Interfaces;
using Hestia.Model.Interfaces;

namespace Hestia.DAL.EFCore.Entities
{
    public static class Extensions
    {
        public static IFileEntity AsEntity(this IFile file) =>
            new FileEntity(file.FullPath,
                           file.LifetimeChanges,
                           file.LifetimeAuthors,
                           file.CoveragePercentage);

        public static IRepositorySnapshotEntity AsEntity(this IRepositorySnapshot snapshot) =>
            new RepositorySnapshotEntity(snapshot.Files.Select(f => f.AsEntity()),
                                         snapshot.AtHash.Match(x => x, string.Empty),
                                         snapshot.CommitCreationDate.Match(x => x, null));
    }
}
