namespace Hestia.Model.Interfaces
{
    public interface IFileGitStats
    {
        int LifetimeChanges { get; }

        int LifetimeAuthors { get; }
    }
}
