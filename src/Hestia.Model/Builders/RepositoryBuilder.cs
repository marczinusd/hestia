using System;
using System.Linq;
using Hestia.Model.Wrappers;
using LanguageExt;
using IO = System.IO;

namespace Hestia.Model.Builders
{
    public static class RepositoryBuilder
    {
        public static Repository BuildRepositoryFromDirectoryPath(string rootPath, IDiskIOWrapper diskIoWrapper)
        {
            PathValidator.ValidateDirectoryPath(rootPath);
            if (!IsGitRepository(rootPath, diskIoWrapper))
            {
                throw new InvalidOperationException($"{rootPath} is not a git repository.");
            }

            return new Repository(-1,
                                  IO.Path.GetDirectoryName(rootPath) ?? throw new InvalidOperationException(),
                                  DirectoryBuilder.BuildDirectoryFromDirectoryPath(rootPath, diskIoWrapper),
                                  Option<string>.None);
        }

        private static bool IsGitRepository(string path, IDiskIOWrapper diskIoWrapper) =>
            diskIoWrapper.EnumerateAllDirectoriesForPath(path)
                         .Any(dir => dir.Contains(".git"));
    }
}
