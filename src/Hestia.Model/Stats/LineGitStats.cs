namespace Hestia.Model.Stats
{
    public class LineGitStats
    {
        // ReSharper disable once UnusedMember.Global
        public LineGitStats()
        {
        }

        public LineGitStats(int modifiedInNumberOfCommits)
        {
            ModifiedInNumberOfCommits = modifiedInNumberOfCommits;
        }

        public long Id { get; set; }

        public int ModifiedInNumberOfCommits { get; }
    }
}
