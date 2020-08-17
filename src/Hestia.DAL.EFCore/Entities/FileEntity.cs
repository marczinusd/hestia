using System.Collections.Generic;
using Hestia.DAL.Interfaces;

namespace Hestia.DAL.EFCore.Entities
{
    public class FileEntity : IFileEntity
    {
        public FileEntity(string path,
                          int lifetimeChanges,
                          int lifetimeAuthors,
                          decimal coveragePercentage,
                          List<ISourceLineEntity> lines)
        {
            Path = path;
            LifetimeChanges = lifetimeChanges;
            LifetimeAuthors = lifetimeAuthors;
            CoveragePercentage = coveragePercentage;
            Lines = lines;
        }

        public string Path { get; }

        public int LifetimeChanges { get; }

        public int LifetimeAuthors { get; }

        public decimal CoveragePercentage { get; }

        public List<ISourceLineEntity> Lines { get; }
    }
}
