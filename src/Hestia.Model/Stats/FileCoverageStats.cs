using System.Linq;
using Newtonsoft.Json;

namespace Hestia.Model.Stats
{
    public class FileCoverageStats
    {
        public FileCoverageStats(FileCoverage coverage)
        {
            Coverage = coverage;
        }

        // Store the coverage for stats, but don't render it unless file details are rendered
        [System.Text.Json.Serialization.JsonIgnore]
        [JsonIgnore]
        public FileCoverage Coverage { get; }

        public decimal PercentageOfLineCoverage =>
            !Coverage.LineCoverages.Any()
                ? 0
                : Coverage.LineCoverages.Count(l => l.HitCount > 0) / (decimal)Coverage.LineCoverages.Count();
    }
}
