namespace Hestia.DAL.Entities
{
    public class LineGitStatsEntity
    {
        public long Id { get; set; }

        public int LineNumber { get; set; }

        public int NumberOfLifetimeAuthors { get; set; }

        public int ModifiedInNumberOfCommits { get; set; }
    }
}
