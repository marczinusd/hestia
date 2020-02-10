namespace Hestia.Model
{
    public class LineGitStats
    {
        public LineGitStats(int modifiedInNumberOfCommites)
        {
            ModifiedInNumberOfCommites = modifiedInNumberOfCommites;
        }

        public int ModifiedInNumberOfCommites { get; }
    }
}
