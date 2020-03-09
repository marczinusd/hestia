using System;
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
                                          DirectoryBuilder.BuildDirectoryFromDirectoryPath(args.SourceRoot,
                                                                                           args.SourceExtensions,
                                                                                           args.DiskIoWrapper,
                                                                                           args.PathValidator),
                                          string.IsNullOrWhiteSpace(args.CoveragePath)
                                              ? Option<string>.None
                                              : Some(args.CoveragePath),
                                          args.AtHash,
                                          args.CommitCreationDate);
        }

        public static RepositorySnapshot Build(this RepositorySnapshotBuilderArguments args) =>
            BuildRepositoryFromDirectoryPath(args);
    }
}
