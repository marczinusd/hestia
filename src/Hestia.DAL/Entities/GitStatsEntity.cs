namespace Hestia.DAL.Entities
{
    public class GitStatsEntity
    {
        public long Id { get; set; }

        public int LifetimeChanges { get; set; }

        public int LifetimeAuthors { get; set; }
    }
}
