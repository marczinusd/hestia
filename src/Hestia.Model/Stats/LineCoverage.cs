using System;
using Hestia.Model.Interfaces;

namespace Hestia.Model.Stats
{
    public sealed class LineCoverage : ILineCoverage
    {
        public LineCoverage(int lineNumber, int hitCount, bool branch, string conditionCoverage)
        {
            LineNumber = lineNumber;
            HitCount = hitCount;
            Branch = branch;
            ConditionCoverage = conditionCoverage;
        }

        public int LineNumber { get; }

        public int HitCount { get; }

        public bool Branch { get; }

        public string ConditionCoverage { get; }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((ILineCoverage)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(LineNumber, HitCount);
        }

        public override string ToString() => $"({LineNumber}, {HitCount})";

        private bool Equals(ILineCoverage other)
        {
            return LineNumber == other.LineNumber && HitCount == other.HitCount && Branch == other.Branch &&
                   ConditionCoverage == other.ConditionCoverage;
        }
    }
}
