using Hestia.Model.Interfaces;
using LanguageExt;
using static LanguageExt.Prelude;

namespace Hestia.Model
{
    public class SourceLine : ISourceLine
    {
        public SourceLine(int lineNumber,
                          string text,
                          Option<ILineCoverageStats> lineCoverageStats,
                          Option<ILineGitStats> lineGitStats)
        {
            LineCoverageStats = lineCoverageStats;
            Text = text;
            LineGitStats = lineGitStats;
            LineNumber = lineNumber;
        }

        public int LineNumber { get; }

        public string Text { get; }

        public Option<ILineCoverageStats> LineCoverageStats { get; }

        public Option<ILineGitStats> LineGitStats { get; }

        public SourceLine With(ILineCoverageStats? coverageStats = null, ILineGitStats? gitStats = null) =>
            new SourceLine(LineNumber,
                           Text,
                           coverageStats != null ? Some(coverageStats) : LineCoverageStats,
                           gitStats != null ? Some(gitStats) : LineGitStats);
    }
}
