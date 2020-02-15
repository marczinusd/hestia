using Hestia.Model.Stats;

namespace Hestia.Model
{
    public class SourceLine
    {
        // EFCore needs parameterless ctors for entities
        // ReSharper disable once UnusedMember.Global
        public SourceLine()
        {
        }

        public SourceLine(string text, LineCoverageStats lineCoverageStats, LineGitStats lineGitStats)
        {
            LineCoverageStats = lineCoverageStats;
            Text = text;
            LineGitStats = lineGitStats;
        }

        public long Id { get; set; }

        public int LineNumber { get; set; }

        public string Text { get; set; }

        public LineCoverageStats LineCoverageStats { get; set; }

        public LineGitStats LineGitStats { get; set; }
    }
}
