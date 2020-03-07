using System.Collections.Generic;

namespace Hestia.Model
{
    public class Directory
    {
        public Directory(string name, string path, IList<Directory> directories, IList<File> files)
        {
            Name = name;
            Path = path;
            Directories = directories;
            Files = files;
        }

        public string Name { get; }

        public string Path { get; }

        public IList<File> Files { get; }

        public IList<Directory> Directories { get; }

        public Directory With(IList<File> files = null, IList<Directory> directories = null) =>
            new Directory(Name,
                          Path,
                          directories ?? Directories,
                          files ?? Files);
    }
}
