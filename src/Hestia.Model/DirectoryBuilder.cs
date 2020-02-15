using System.Linq;

namespace Hestia.Model
{
    public static class DirectoryBuilder
    {
        public static Directory BuildDirectoryStructureFromFilePath(string filepath)
        {
            return new Directory(0, "bla", filepath,
                                 Enumerable.Empty<Directory>(),
                                 Enumerable.Empty<File>());
        }
    }
}
