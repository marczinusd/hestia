namespace Hestia.Model.Stats
{
    public class LineCoverage
    {
        public LineCoverage(int lineNumber, int hitCount)
        {
            LineNumber = lineNumber;
            HitCount = hitCount;
        }

        public int LineNumber { get; }

        public int HitCount { get; }

        public override string ToString() => $"({LineNumber}, {HitCount})";
    }
}
