using System.Collections.Generic;

namespace Hestia.DAL.Entities
{
    public class FileEntity
    {
        public long Id { get; set; }

        public ICollection<LineEntity> Content { get; set; } = new List<LineEntity>();

        public string Path { get; set; }

        public string Filename { get; set; }

        public string Extension { get; set; }

        public GitStatsEntity GitStats { get; set; }

        public CoverageEntity CoverageStats { get; set; }
    }
}
