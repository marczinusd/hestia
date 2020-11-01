namespace Hestia.DAL.Interfaces
{
    public interface ILineEntity
    {
        string Content { get; }

        bool IsCovered { get; }

        bool IsBranched { get; }

        string ConditionCoverage { get; }

        int HitCount { get; }

        int NumberOfAuthors { get; }

        int NumberOfChanges { get; }

        int LineNumber { get; }
    }
}
