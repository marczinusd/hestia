using Hestia.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

#pragma warning disable 8618

namespace Hestia.DAL.EFCore
{
    public class HestiaContext : DbContext
    {
        public DbSet<IRepositorySnapshotEntity> Snapshots { get; set; }

        public DbSet<IFileEntity> Files { get; set; }

        public DbSet<ISourceLineEntity> SourceLines { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite("Data Source=hestia.db");
    }
}
