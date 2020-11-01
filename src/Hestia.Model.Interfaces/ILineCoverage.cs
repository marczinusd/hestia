namespace Hestia.Model.Interfaces
{
    public interface ILineCoverage
    {
        int LineNumber { get; }

        int HitCount { get; }

        bool Branch { get; }

        string ConditionCoverage { get; }
    }
}
