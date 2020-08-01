using Hestia.Model.Interfaces;

namespace Hestia.Model.Stats
{
    public class LineCoverageStats : ILineCoverageStats
    {
        public LineCoverageStats(bool isCovered)
        {
            IsCovered = isCovered;
        }

        public bool IsCovered { get; }
    }
}
