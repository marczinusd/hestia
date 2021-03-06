using System.IO;
using System.Linq;
using Hestia.Model.Interfaces;
using LanguageExt;
using static LanguageExt.Prelude;

namespace Hestia.Model.Builders
{
    public static class RepositorySnapshotBuilder
    {
        public static IRepositorySnapshot Build(this RepositorySnapshotBuilderArguments args)
        {
            args.PathValidator.ValidateDirectoryPath(args.RootPath);
            args.PathValidator.ValidateDirectoryPath(Path.Join(args.RootPath, args.SourceRoot));

            return new RepositorySnapshot(args.SnapshotId,
                                          args.RootPath,
                                          args.DiskIoWrapper
                                              .EnumerateAllFilesForPathRecursively(Path.Join(args.RootPath,
                                                  args.SourceRoot))
                                              .Where(f => !args.SourceExtensions.Any() ||
                                                          args.SourceExtensions.Contains(Path.GetExtension(f)))
                                              .Select(filePath =>
                                                          FileBuilder.BuildFileFromPath(filePath, args.DiskIoWrapper))
                                              .ToList(),
                                          new DirectoryInfo(args.RootPath).Name,
                                          string.IsNullOrWhiteSpace(args.CoveragePath)
                                              ? Option<string>.None
                                              : Some(args.CoveragePath),
                                          args.AtHash,
                                          args.CommitCreationDate,
                                          Option<int>.None,
                                          Option<int>.None);
        }
    }
}
