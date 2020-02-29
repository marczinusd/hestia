using System.Collections.Generic;
using System.Linq;

namespace Hestia.Model.Stats
{
    public class FileCoverage
    {
        public FileCoverage(string fileName,
                            IEnumerable<(int lineNumber, int hitCount)> lineCoverages)
        {
            FileName = fileName;
            LineCoverages = lineCoverages;
        }

        public string FileName { get; }

        public IEnumerable<(int lineNumber, int hitCount)> LineCoverages { get; }

        public override string ToString() =>
            $"{FileName} : {string.Join(" ", LineCoverages.Select(lc => $"({lc.lineNumber}, {lc.hitCount}"))}";
    }
}
