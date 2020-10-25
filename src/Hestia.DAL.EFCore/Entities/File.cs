using System.Collections.Generic;
using JetBrains.Annotations;

namespace Hestia.DAL.EFCore.Entities
{
    public class File
    {
        [UsedImplicitly]
        public File()
        {
        }

        public File(string path,
                    int lifetimeChanges,
                    int lifetimeAuthors,
                    decimal coveragePercentage,
                    List<Line> lines,
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

        [UsedImplicitly] public List<Line> Lines { get; set; }

        [UsedImplicitly] public Snapshot Snapshot { get; set; }

        [UsedImplicitly] public string SnapshotId { get; set; }
    }
}
