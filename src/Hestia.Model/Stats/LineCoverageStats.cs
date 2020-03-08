namespace Hestia.Model.Stats
{
    public class LineCoverageStats
    {
        // ReSharper disable once UnusedMember.Global
        public LineCoverageStats()
        {
        }

        public LineCoverageStats(bool isCovered)
        {
            IsCovered = isCovered;
        }

        public long Id { get; set; }

        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        public bool IsCovered { get; }
    }
}
