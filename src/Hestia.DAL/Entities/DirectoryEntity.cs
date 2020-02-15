using System.Collections.Generic;

namespace Hestia.DAL.Entities
{
    public class DirectoryEntity
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Path { get; set; }

        public ICollection<FileEntity> Files { get; set; } = new List<FileEntity>();

        public ICollection<DirectoryEntity> Directories { get; set; } = new List<DirectoryEntity>();
    }
}
