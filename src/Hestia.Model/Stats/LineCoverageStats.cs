using Hestia.Model.Interfaces;

namespace Hestia.Model.Stats
{
    public class LineCoverageStats : ILineCoverageStats
    {
        public LineCoverageStats(bool isCovered, int hitCount, bool isBranched, string conditionCoverage)
        {
            IsCovered = isCovered;
            HitCount = hitCount;
            IsBranched = isBranched;
            ConditionCoverage = conditionCoverage;
        }

        public bool IsCovered { get; }

        public int HitCount { get; }

        public bool IsBranched { get; }

        public string ConditionCoverage { get; }
    }
}
