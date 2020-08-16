using Hestia.Model.Interfaces;

namespace Hestia.Model
{
    public class FileHeader : IFileHeader
    {
        public FileHeader(string path, decimal coveragePercentage, int lifetimeAuthors, int lifetimeChanges)
        {
            Path = path;
            CoveragePercentage = coveragePercentage;
            LifetimeAuthors = lifetimeAuthors;
            LifetimeChanges = lifetimeChanges;
        }

        public string Path { get; }

        public decimal CoveragePercentage { get; }

        public int LifetimeAuthors { get; }

        public int LifetimeChanges { get; }
    }
}
