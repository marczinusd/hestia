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
                                          args.DiskIoWrapper
                                              .EnumerateAllFilesForPathRecursively(Path.Join(args.RootPath,
                                                  args.SourceRoot))
                                              .Select(filePath =>
                                                          FileBuilder.BuildFileFromPath(filePath, args.DiskIoWrapper))
                                              .Where(f => !args.SourceExtensions.Any() ||
                                                          args.SourceExtensions.Contains(f.Extension))
                                              .ToList(),
                                          string.IsNullOrWhiteSpace(args.CoveragePath)
                                              ? Option<string>.None
                                              : Some(args.CoveragePath),
                                          args.AtHash,
                                          args.CommitCreationDate,
                                          new DirectoryInfo(args.RootPath).Name,
                                          args.RootPath);
        }
    }
}
