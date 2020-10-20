using Hestia.DAL.EFCore.Entities;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

#pragma warning disable 8618

namespace Hestia.DAL.EFCore
{
    public class HestiaContext : DbContext
    {
        public HestiaContext(DbContextOptions options)
            : base(options)
        {
        }

        [UsedImplicitly] public DbSet<Snapshot> Snapshots { get; set; }

        [UsedImplicitly] public DbSet<File> Files { get; set; }

        [UsedImplicitly] public DbSet<Line> SourceLines { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Line>()
                        .Property(l => l.Id)
                        .ValueGeneratedOnAdd();
            modelBuilder.Entity<Line>()
                        .HasOne(l => l.File)
                        .WithMany(p => p.Lines);

            modelBuilder.Entity<Snapshot>()
                        .Property(s => s.Id)
                        .ValueGeneratedOnAdd();
            modelBuilder.Entity<Snapshot>()
                        .HasMany(r => r.Files)
                        .WithOne(f => f.Snapshot);

            modelBuilder.Entity<File>()
                        .Property(f => f.Id)
                        .ValueGeneratedOnAdd();
            modelBuilder.Entity<File>()
                        .HasOne(f => f.Snapshot)
                        .WithMany(parent => parent.Files);
        }
    }
}
