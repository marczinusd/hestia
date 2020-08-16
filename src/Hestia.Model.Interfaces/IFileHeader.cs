namespace Hestia.Model.Interfaces
{
    public interface IFileHeader
    {
        string Path { get; }

        decimal CoveragePercentage { get; }

        int LifetimeAuthors { get; }

        int LifetimeChanges { get; }
    }
}
