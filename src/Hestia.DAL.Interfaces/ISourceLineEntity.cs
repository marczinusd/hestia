namespace Hestia.DAL.Interfaces
{
    public interface ISourceLineEntity
    {
        string Content { get; }

        bool IsCovered { get; }

        int NumberOfAuthors { get; }

        int NumberOfChanges { get; }
    }
}
