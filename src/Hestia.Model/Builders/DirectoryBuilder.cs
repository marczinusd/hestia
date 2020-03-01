using System.IO;
using System.Linq;
using Hestia.Model.Wrappers;

namespace Hestia.Model.Builders
{
    public static class DirectoryBuilder
    {
        public static Directory BuildDirectoryFromDirectoryPath(string pathToDirectory,
                                                                string[] sourceExtensions,
                                                                IDiskIOWrapper diskIoWrapper,
                                                                IPathValidator pathValidator)
        {
            pathValidator.ValidateDirectoryPath(pathToDirectory);

            var directories = diskIoWrapper.EnumerateAllDirectoriesForPath(pathToDirectory);
            var files = diskIoWrapper.EnumerateAllFilesForPath(pathToDirectory);

            return new Directory(GetDirectoryName(pathToDirectory),
                                 pathToDirectory,
                                 directories
                                     .Select(dir => BuildDirectoryFromDirectoryPath(Path.Join(pathToDirectory,
                                                                                              GetDirectoryName(dir)),
                                                                                    sourceExtensions,
                                                                                    diskIoWrapper,
                                                                                    pathValidator))
                                     .Where(dir => dir.Files.Any()) // filter directories with no source files
                                     .ToList(),
                                 files.Where(file => !sourceExtensions.Any() || sourceExtensions.Contains(Path.GetExtension(file)))
                                      .Select(file =>
                                                  FileBuilder.BuildFileFromPath(Path.Join(pathToDirectory,
                                                                                          Path.GetFileName(file)),
                                                                                diskIoWrapper))
                                      .ToList());
        }

        private static string GetDirectoryName(string fullDirectoryPath) =>
            fullDirectoryPath
                .Split(Path.DirectorySeparatorChar)
                .Last();
    }
}
