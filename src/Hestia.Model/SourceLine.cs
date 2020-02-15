using Hestia.Model.Stats;

namespace Hestia.Model
{
    public class SourceLine
    {
        public SourceLine(int lineNumber,
                          string text,
                          LineCoverageStats lineCoverageStats,
                          LineGitStats lineGitStats)
        {
            LineCoverageStats = lineCoverageStats;
            Text = text;
            LineGitStats = lineGitStats;
            LineNumber = lineNumber;
        }

        public int LineNumber { get; }

        public string Text { get; }

        public LineCoverageStats LineCoverageStats { get; }

        public LineGitStats LineGitStats { get; }
    }
}
