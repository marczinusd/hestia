namespace Hestia.Model.Interfaces
{
    public interface ILineGitStats
    {
        int LineNumber { get; }

        int ModifiedInNumberOfCommits { get; }

        int NumberOfLifetimeAuthors { get; }
    }
}
