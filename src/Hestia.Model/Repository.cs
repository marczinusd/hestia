using LanguageExt;

namespace Hestia.Model
{
    public class Repository
    {
        public Repository(string name, Directory rootDirectory, Option<string> pathToCoverageResultFile)
        {
            Name = name;
            RootDirectory = rootDirectory;
            PathToCoverageResultFile = pathToCoverageResultFile;
        }

        public string Name { get; }

        public Option<string> PathToCoverageResultFile { get; }

        public Directory RootDirectory { get; }
    }
}
