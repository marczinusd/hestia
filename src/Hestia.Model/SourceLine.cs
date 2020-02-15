using Hestia.Model.Stats;

namespace Hestia.Model
{
    public class SourceLine
    {
        public SourceLine(string text, LineCoverageStats lineCoverageStats, LineGitStats lineGitStats)
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
