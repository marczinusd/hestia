using System.Linq;
using System.Text.Json.Serialization;
using Hestia.Model.Interfaces;

namespace Hestia.Model.Stats
{
    public class FileCoverageStats : IFileCoverageStats
    {
        public FileCoverageStats(IFileCoverage coverage) => Coverage = coverage;

        // Store the coverage for stats, but don't render it unless file details are rendered
        [JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public IFileCoverage Coverage { get; }

        public decimal PercentageOfLineCoverage =>
            !Coverage.LineCoverages.Any()
                ? 0
                : Coverage.LineCoverages.Count(l => l.HitCount > 0) / (decimal)Coverage.LineCoverages.Count() * 100;
    }
}
