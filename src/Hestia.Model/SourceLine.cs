namespace Hestia.Model
{
    public class SourceLine
    {
        public SourceLine(LineCoverageStats lineCoverageStats, string text, LineGitStats lineGitStats)
        {
            LineCoverageStats = lineCoverageStats;
            Text = text;
            LineGitStats = lineGitStats;
        }

        public string Text { get; }

        public LineCoverageStats LineCoverageStats { get; }

        public LineGitStats LineGitStats { get; }
    }
}
