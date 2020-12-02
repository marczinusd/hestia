namespace Hestia.Model.Interfaces
{
    public interface IFileHeader
    {
        string Id { get; }

        string Path { get; }

        decimal CoveragePercentage { get; }

        int LifetimeAuthors { get; }

        int LifetimeChanges { get; }

        int LineCount { get; }
    }
}
