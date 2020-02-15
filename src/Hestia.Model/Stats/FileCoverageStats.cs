namespace Hestia.Model.Stats
{
    public class FileCoverageStats
    {
        public FileCoverageStats(decimal percentageOfLineCoverage)
        {
            PercentageOfLineCoverage = percentageOfLineCoverage;
        }

        public decimal PercentageOfLineCoverage { get; }
    }
}
