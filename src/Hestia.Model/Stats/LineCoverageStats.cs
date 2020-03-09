namespace Hestia.Model.Stats
{
    public class LineCoverageStats
    {
        public LineCoverageStats(bool isCovered)
        {
            IsCovered = isCovered;
        }

        public bool IsCovered { get; }
    }
}
