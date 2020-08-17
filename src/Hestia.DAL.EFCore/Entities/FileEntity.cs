using System.Collections.Generic;
using JetBrains.Annotations;

namespace Hestia.DAL.EFCore.Entities
{
    public class FileEntity
    {
        [UsedImplicitly]
        public FileEntity()
        {
        }

        public FileEntity(string path,
                          int lifetimeChanges,
                          int lifetimeAuthors,
                          decimal coveragePercentage,
                          List<LineEntity> lines,
                          string id)
        {
            Path = path;
            LifetimeChanges = lifetimeChanges;
            LifetimeAuthors = lifetimeAuthors;
            CoveragePercentage = coveragePercentage;
            Lines = lines;
            Id = id;
        }

        public string Id { get; set; }

        public string Path { get; set; }

        public int LifetimeChanges { get; set; }

        public int LifetimeAuthors { get; set; }

        public decimal CoveragePercentage { get; set; }

        public List<LineEntity> Lines { get; set; }
    }
}
