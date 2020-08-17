using System;
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
                           file.CoveragePercentage,
                           file.Content
                               .Select(AsEntity)
                               .ToList(),
                           null);

        public static IRepositorySnapshotEntity AsEntity(this IRepositorySnapshot snapshot) =>
            new RepositorySnapshotEntity(snapshot.Files.Select(f => f.AsEntity()),
                                         snapshot.AtHash.Match(x => x, string.Empty),
                                         snapshot.CommitCreationDate.Match(x => x, () => null as DateTime?),
                                         snapshot.RepositoryName.Match(x => x, () => null!),
                                         null!);

        public static ISourceLineEntity AsEntity(this ISourceLine line) =>
            new LineEntity(line.Text,
                           line.LineCoverageStats.Match(x => x.IsCovered,
                                                        () => false),
                           line.LineGitStats.Match(x => x.NumberOfLifetimeAuthors,
                                                   () => 0),
                           line.LineGitStats.Match(x => x.ModifiedInNumberOfCommits,
                                                   () => 0));
    }
}
