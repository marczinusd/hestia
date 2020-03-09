namespace Hestia.Model.Stats
{
    public class LineGitStats
    {
        public LineGitStats(int lineNumber, int modifiedInNumberOfCommits, int numberOfLifetimeAuthors)
        {
            LineNumber = lineNumber;
            ModifiedInNumberOfCommits = modifiedInNumberOfCommits;
            NumberOfLifetimeAuthors = numberOfLifetimeAuthors;
        }

        public int LineNumber { get; }

        public int ModifiedInNumberOfCommits { get; }

        public int NumberOfLifetimeAuthors { get; }
    }
}
