namespace Hestia.DAL.Entities
{
    public class LineEntity
    {
        public long Id { get; set; }

        public int LineNumber { get; set; }

        public string Text { get; set; }

        public LineCoverageEntity LineCoverageStats { get; set; }

        public LineGitStatsEntity LineGitStats { get; set; }
    }
}
