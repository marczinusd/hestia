namespace Hestia.Model.Interfaces
{
    public interface IFileCoverageStats
    {
        IFileCoverage Coverage { get; }

        decimal PercentageOfLineCoverage { get; }
    }
}
