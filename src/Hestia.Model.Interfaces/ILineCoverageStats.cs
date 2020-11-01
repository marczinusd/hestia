namespace Hestia.Model.Interfaces
{
    public interface ILineCoverageStats
    {
        bool IsCovered { get; }

        int HitCount { get; }

        bool IsBranched { get; }

        string ConditionCoverage { get; }
    }
}
