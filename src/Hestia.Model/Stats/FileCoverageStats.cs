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
            !Coverage.LineCoverages.Any()
                ? 0
                : Coverage.LineCoverages.Count(l => l.HitCount > 0) / (decimal)Coverage.LineCoverages.Count();
    }
}
