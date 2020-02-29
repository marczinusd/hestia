using System.Linq;

namespace Hestia.Model.Stats
{
    public class FileCoverageStats
    {
        public FileCoverageStats(FileCoverage coverage)
        {
            Coverage = coverage;
        }

        public FileCoverage Coverage { get; }

        public decimal PercentageOfLineCoverage =>
            Coverage.LineCoverages.Count() / (decimal)Coverage.LineCoverages.Count(l => l.hitCount == 0);
    }
}
