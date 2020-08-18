namespace Hestia.DAL.Interfaces
{
    public interface ILineEntity
    {
        string Content { get; }

        bool IsCovered { get; }

        int NumberOfAuthors { get; }

        int NumberOfChanges { get; }

        int LineNumber { get; }
    }
}
