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

        [UsedImplicitly] public DbSet<RepositorySnapshotEntity> Snapshots { get; set; }

        [UsedImplicitly] public DbSet<FileEntity> Files { get; set; }

        [UsedImplicitly] public DbSet<LineEntity> SourceLines { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LineEntity>()
                        .Property(l => l.Id)
                        .ValueGeneratedOnAdd();
            modelBuilder.Entity<LineEntity>()
                        .HasOne(l => l.Parent)
                        .WithMany(p => p.Lines);

            modelBuilder.Entity<RepositorySnapshotEntity>()
                        .Property(s => s.Id)
                        .ValueGeneratedOnAdd();
            modelBuilder.Entity<RepositorySnapshotEntity>()
                        .HasMany(r => r.Files)
                        .WithOne(f => f.Parent);

            modelBuilder.Entity<FileEntity>()
                        .Property(f => f.Id)
                        .ValueGeneratedOnAdd();
            modelBuilder.Entity<FileEntity>()
                        .HasOne(f => f.Parent)
                        .WithMany(parent => parent.Files);
        }
    }
}
