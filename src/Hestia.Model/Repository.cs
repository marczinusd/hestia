using LanguageExt;

namespace Hestia.Model
{
    public class Repository
    {
        public Repository(string name, Directory rootDirectory, Option<string> pathToCoverageResultFile, long id)
        {
            Name = name;
            RootDirectory = rootDirectory;
            PathToCoverageResultFile = pathToCoverageResultFile;
            Id = id;
        }

        public string Name { get; }

        public Option<string> PathToCoverageResultFile { get; }

        public Directory RootDirectory { get; }

        public long Id { get; }

        public RepositoryIdentifier AsRepositoryIdentifier() => new RepositoryIdentifier(Id, Name);
    }
}
