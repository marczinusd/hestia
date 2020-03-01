using System;
using System.IO;
using System.Linq;
using Hestia.Model.Wrappers;
using LanguageExt;
using static LanguageExt.Prelude;

namespace Hestia.Model.Builders
{
    public static class RepositoryBuilder
    {
        public static Repository BuildRepositoryFromDirectoryPath(long repoId,
                                                                  string name,
                                                                  string rootPath,
                                                                  string sourceRoot,
                                                                  string[] sourceExtensions,
                                                                  string coveragePath,
                                                                  IDiskIOWrapper diskIoWrapper,
                                                                  IPathValidator pathValidator)
        {
            pathValidator.ValidateDirectoryPath(rootPath);
            pathValidator.ValidateDirectoryPath(sourceRoot);
            if (!IsGitRepository(rootPath, diskIoWrapper))
            {
                throw new InvalidOperationException($"{rootPath} is not a git repository.");
            }

            return new Repository(repoId,
                                  !string.IsNullOrWhiteSpace(name) ? name : Path.GetDirectoryName(sourceRoot) ?? throw new InvalidOperationException(),
                                  DirectoryBuilder.BuildDirectoryFromDirectoryPath(sourceRoot,
                                                                                   sourceExtensions,
                                                                                   diskIoWrapper,
                                                                                   pathValidator),
                                  string.IsNullOrWhiteSpace(coveragePath) ? Option<string>.None : Some(coveragePath));
        }

        private static bool IsGitRepository(string path, IDiskIOWrapper diskIoWrapper) =>
            diskIoWrapper.EnumerateAllDirectoriesForPath(path)
                         .Any(dir => dir.Contains(".git"));
    }
}
