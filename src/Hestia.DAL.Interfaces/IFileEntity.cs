namespace Hestia.DAL.Interfaces
{
    public interface IFileEntity
    {
        string Path { get; }

        int LifetimeChanges { get; }

        int LifetimeAuthors { get; }

        decimal CoveragePercentage { get; }
    }
}
