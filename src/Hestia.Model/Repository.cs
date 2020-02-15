using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LanguageExt;

namespace Hestia.Model
{
    public class Repository
    {
        // EFCore needs parameterless ctors for entities
        // ReSharper disable once UnusedMember.Global
        public Repository()
        {
        }

        public Repository(long id, string name, Directory rootDirectory, Option<string> pathToCoverageResultFile)
        {
            Name = name;
            RootDirectory = rootDirectory;
            PathToCoverageResultFile = pathToCoverageResultFile;
            Id = id;
        }

        public string Name { get; set; }

        [NotMapped] public Option<string> PathToCoverageResultFile { get; set; }

        public Directory RootDirectory { get; set; }

        [Key]
        public long Id { get; set; }

        public RepositoryIdentifier AsRepositoryIdentifier() => new RepositoryIdentifier(Id, Name);
    }
}
