using System.ComponentModel.DataAnnotations.Schema;
using LanguageExt;

namespace Hestia.Model
{
    public class Repository
    {
        public Repository(long id, string name, Directory rootDirectory, Option<string> pathToCoverageResultFile)
        {
            Name = name;
            RootDirectory = rootDirectory;
            PathToCoverageResultFile = pathToCoverageResultFile;
            Id = id;
        }

        public string Name { get; }

        [NotMapped] public Option<string> PathToCoverageResultFile { get; }

        public Directory RootDirectory { get; }

        public long Id { get; }

        public RepositoryIdentifier AsRepositoryIdentifier() => new RepositoryIdentifier(Id, Name);
    }
}
