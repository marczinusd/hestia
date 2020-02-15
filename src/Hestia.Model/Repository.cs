namespace Hestia.Model
{
    public class Repository
    {
        public Repository(string name, Directory rootDirectory)
        {
            Name = name;
            RootDirectory = rootDirectory;
        }

        public string Name { get; }

        public Directory RootDirectory { get; }
    }
}
