namespace Hestia.Model
{
    public class LineCoverageStats
    {
        public LineCoverageStats(bool isCovered)
        {
            IsCovered = isCovered;
        }

        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        private bool IsCovered { get; }
    }
}
