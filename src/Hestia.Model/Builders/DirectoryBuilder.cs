using System.IO;
using System.Linq;
using Hestia.Model.Wrappers;

namespace Hestia.Model.Builders
{
    public static class DirectoryBuilder
    {
        public static Directory BuildDirectoryFromDirectoryPath(string pathToDirectory, IDiskIOWrapper diskIoWrapper)
        {
            PathValidator.ValidateDirectoryPath(pathToDirectory);

            var directories = diskIoWrapper.EnumerateAllDirectoriesForPath(pathToDirectory);
            var files = diskIoWrapper.EnumerateAllFilesForPath(pathToDirectory);

            return new Directory(Path.GetDirectoryName(pathToDirectory),
                                 Path.GetPathRoot(pathToDirectory),
                                 directories.Select(dir => BuildDirectoryFromDirectoryPath(Path.Join(pathToDirectory, dir), diskIoWrapper)),
                                 files.Select(file => FileBuilder.BuildFileFromPath(Path.Join(pathToDirectory, file), diskIoWrapper)));
        }
    }
}
