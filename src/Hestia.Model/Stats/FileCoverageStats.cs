namespace Hestia.Model.Stats
{
    public class FileCoverageStats
    {
        // ReSharper disable once UnusedMember.Global
        public FileCoverageStats()
        {
        }

        public FileCoverageStats(decimal percentageOfLineCoverage)
        {
            PercentageOfLineCoverage = percentageOfLineCoverage;
        }

        public long Id { get; set; }

        public decimal PercentageOfLineCoverage { get; }
    }
}
