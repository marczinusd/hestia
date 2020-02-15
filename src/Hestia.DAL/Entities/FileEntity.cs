using Microsoft.EntityFrameworkCore;

namespace Hestia.DAL.Entities
{
    public class FileEntity
    {
        public long Id { get; set; }

        public DbSet<LineEntity> Content { get; set; }

        public string Path { get; set; }

        public string Filename { get; set; }

        public string Extension { get; set; }

        public GitStatsEntity GitStats { get; set; }

        public CoverageEntity CoverageStats { get; set; }
    }
}
