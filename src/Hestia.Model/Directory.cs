using System.Collections.Generic;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
namespace Hestia.Model
{
    public class Directory
    {
        // EFCore needs parameterless ctors for entities
        // ReSharper disable once UnusedMember.Global
        public Directory()
        {
        }

        public Directory(long id, string name, string path, IEnumerable<Directory> directories, IEnumerable<File> files)
        {
            Name = name;
            Path = path;
            Directories = directories;
            Files = files;
            Id = id;
        }

        public long Id { get; set; }

        public string Name { get; set; }

        public string Path { get; set; }

        public IEnumerable<File> Files { get; set; }

        public IEnumerable<Directory> Directories { get; set; }
    }
}
