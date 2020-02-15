using Microsoft.EntityFrameworkCore;

namespace Hestia.DAL.Entities
{
    public class DirectoryEntity
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Path { get; set; }

        public DbSet<FileEntity> Files { get; set; }

        public DbSet<DirectoryEntity> Directories { get; set; }
    }
}
