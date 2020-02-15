using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Hestia.Model;
using LanguageExt;
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
            Repositories.Add(new Repository(
                                            "bla",
                                            new Directory("blabla",
                                                          "/",
                                                          Enumerable.Empty<Directory>(),
                                                          Enumerable.Empty<File>()),
                                            Option<string>.None));
        }

        [NotNull] public DbSet<Repository> Repositories { get; set; }
    }
#nullable restore
}
