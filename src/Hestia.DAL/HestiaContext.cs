using System.Diagnostics.CodeAnalysis;
using Hestia.DAL.Entities;
using Hestia.Model;
using Microsoft.EntityFrameworkCore;

namespace Hestia.DAL
{
#nullable disable
    // ReSharper disable once ClassNeverInstantiated.Global
    public class HestiaContext : DbContext
    {
        public HestiaContext(DbContextOptions<HestiaContext> options)
            : base(options)
        {
        }

        [NotNull] public DbSet<RepositoryEntity> Repositories { get; set; }
    }
#nullable restore
}
