namespace Hestia.DAL.Entities
{
    public class RepositoryEntity
    {
        public string Name { get; set; }

        public string PathToCoverageResultFile { get; set; }

        public DirectoryEntity RootDirectory { get; set; }

        public long Id { get; set; }
    }
}
