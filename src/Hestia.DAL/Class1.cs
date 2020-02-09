using Microsoft.EntityFrameworkCore;

namespace Hestia.DAL
{
    /// <summary>
    /// Class1.
    /// </summary>
    // ReSharper disable once UnusedType.Global
    public class Class1 : DbContext
    {
        /// <inheritdoc/>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite("todo");
        }
    }
}
