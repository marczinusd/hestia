using System;

namespace Hestia.Model
{
    public class Repository
    {
        public Repository(string name, Directory rootDirectory)
        {
            Name = name;
            RootDirectory = rootDirectory;
        }

        public string Name { get; private set; }

        public Directory RootDirectory { get; private set; }
    }
}
