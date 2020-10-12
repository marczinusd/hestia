using System;
using Hestia.Model.Interfaces;

namespace Hestia.Model.Stats
{
    public sealed class LineCoverage : ILineCoverage
    {
        public LineCoverage(int lineNumber, int hitCount)
        {
            LineNumber = lineNumber;
            HitCount = hitCount;
        }

        public int LineNumber { get; }

        public int HitCount { get; }

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
            return LineNumber == other.LineNumber && HitCount == other.HitCount;
        }
    }
}
