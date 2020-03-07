using System;
using System.Linq;
using Hestia.Model.Wrappers;
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
            if (!IsGitRepository(args.RootPath, args.DiskIoWrapper))
            {
                throw new InvalidOperationException($"{args.RootPath} is not a git repository.");
            }

            return new RepositorySnapshot(args.RepoId,
                                          DirectoryBuilder.BuildDirectoryFromDirectoryPath(args.SourceRoot,
                                                                                           args.SourceExtensions,
                                                                                           args.DiskIoWrapper,
                                                                                           args.PathValidator),
                                          string.IsNullOrWhiteSpace(args.CoveragePath)
                                              ? Option<string>.None
                                              : Some(args.CoveragePath),
                                          args.AtHash);
        }

        public static RepositorySnapshot Build(this RepositorySnapshotBuilderArguments args) =>
            BuildRepositoryFromDirectoryPath(args);

        private static bool IsGitRepository(string path, IDiskIOWrapper diskIoWrapper) =>
            diskIoWrapper.EnumerateAllDirectoriesForPath(path)
                         .Any(dir => dir.Contains(".git"));
    }
}
