using System.Collections.Generic;

namespace Hestia.Model
{
    public class Directory
    {
        public Directory(string name, string path, IEnumerable<Directory> directories, IEnumerable<File> files)
        {
            Name = name;
            Path = path;
            Directories = directories;
            Files = files;
        }

        public string Name { get; }

        public string Path { get; }

        public IEnumerable<File> Files { get; }

        public IEnumerable<Directory> Directories { get; }
    }
}
