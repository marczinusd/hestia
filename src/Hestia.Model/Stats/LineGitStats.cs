namespace Hestia.Model.Stats
{
    public class LineGitStats
    {
        public LineGitStats(int modifiedInNumberOfCommits)
        {
            ModifiedInNumberOfCommits = modifiedInNumberOfCommits;
        }

        public int ModifiedInNumberOfCommits { get; }
    }
}
