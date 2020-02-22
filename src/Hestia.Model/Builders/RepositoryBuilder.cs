using System;
using System.Linq;
using Hestia.Model.Wrappers;
using LanguageExt;
using IO = System.IO;

namespace Hestia.Model.Builders
{
    public static class RepositoryBuilder
    {
        public static Repository BuildRepositoryFromDirectoryPath(long repoId,
                                                                  string rootPath,
                                                                  IDiskIOWrapper diskIoWrapper,
                                                                  IPathValidator pathValidator)
        {
            pathValidator.ValidateDirectoryPath(rootPath);
            if (!IsGitRepository(rootPath, diskIoWrapper))
            {
                throw new InvalidOperationException($"{rootPath} is not a git repository.");
            }

            return new Repository(repoId,
                                  IO.Path.GetDirectoryName(rootPath) ?? throw new InvalidOperationException(),
                                  DirectoryBuilder.BuildDirectoryFromDirectoryPath(rootPath,
                                                                                   diskIoWrapper,
                                                                                   pathValidator),
                                  Option<string>.None);
        }

        private static bool IsGitRepository(string path, IDiskIOWrapper diskIoWrapper) =>
            diskIoWrapper.EnumerateAllDirectoriesForPath(path)
                         .Any(dir => dir.Contains(".git"));
    }
}
