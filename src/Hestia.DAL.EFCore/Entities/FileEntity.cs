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
                          IList<LineEntity> lines,
                          string id)
        {
            Path = path;
            LifetimeChanges = lifetimeChanges;
            LifetimeAuthors = lifetimeAuthors;
            CoveragePercentage = coveragePercentage;
            Lines = lines;
            Id = id;
        }

        [UsedImplicitly] public string Id { get; set; }

        [UsedImplicitly] public string Path { get; set; }

        [UsedImplicitly] public int LifetimeChanges { get; set; }

        [UsedImplicitly] public int LifetimeAuthors { get; set; }

        [UsedImplicitly] public decimal CoveragePercentage { get; set; }

        [UsedImplicitly] public IList<LineEntity> Lines { get; set; }

        [UsedImplicitly] public RepositorySnapshotEntity Parent { get; set; }
    }
}
