using System.Linq;
using LanguageExt;
using static LanguageExt.Prelude;

namespace Hestia.Model.Builders
{
    public static class RepositorySnapshotBuilder
    {
        public static RepositorySnapshot BuildRepositoryFromDirectoryPath(RepositorySnapshotBuilderArguments args)
        {
            args.PathValidator.ValidateDirectoryPath(args.RootPath);
            args.PathValidator.ValidateDirectoryPath(args.SourceRoot);

            return new RepositorySnapshot(args.SnapshotId,
                                          string.IsNullOrWhiteSpace(args.CoveragePath)
                                              ? Option<string>.None
                                              : Some(args.CoveragePath),
                                          args.AtHash,
                                          args.CommitCreationDate,
                                          args.DiskIoWrapper.EnumerateAllFilesForPathRecursively(args.RootPath)
                                              .Select(filePath =>
                                                          FileBuilder.BuildFileFromPath(filePath, args.DiskIoWrapper))
                                              .Where(f => !args.SourceExtensions.Any() ||
                                                          args.SourceExtensions.Contains(f.Extension))
                                              .ToList());
        }

        public static RepositorySnapshot Build(this RepositorySnapshotBuilderArguments args) =>
            BuildRepositoryFromDirectoryPath(args);
    }
}
