using Hestia.DAL.Interfaces;

namespace Hestia.DAL.EFCore.Entities
{
    public class FileEntity : IFileEntity
    {
        public FileEntity(string path, int lifetimeChanges, int lifetimeAuthors, decimal coveragePercentage)
        {
            Path = path;
            LifetimeChanges = lifetimeChanges;
            LifetimeAuthors = lifetimeAuthors;
            CoveragePercentage = coveragePercentage;
        }

        public string Path { get; }

        public int LifetimeChanges { get; }

        public int LifetimeAuthors { get; }

        public decimal CoveragePercentage { get; }
    }
}
