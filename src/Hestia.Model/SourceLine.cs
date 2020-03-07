using Hestia.Model.Stats;
using LanguageExt;

namespace Hestia.Model
{
    public class SourceLine
    {
        public SourceLine(int lineNumber,
                          string text,
                          Option<LineCoverageStats> lineCoverageStats,
                          Option<LineGitStats> lineGitStats)
        {
            LineCoverageStats = lineCoverageStats;
            Text = text;
            LineGitStats = lineGitStats;
            LineNumber = lineNumber;
        }

        public int LineNumber { get; }

        public string Text { get; }

        public Option<LineCoverageStats> LineCoverageStats { get; }

        public Option<LineGitStats> LineGitStats { get; }

        public SourceLine With(LineCoverageStats? coverageStats = null, LineGitStats? gitStats = null) =>
            new SourceLine(LineNumber,
                           Text,
                           coverageStats ?? LineCoverageStats,
                           gitStats ?? LineGitStats);
    }
}
