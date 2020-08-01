using System.Collections.Generic;
using System.Linq;
using Hestia.Model.Interfaces;

namespace Hestia.Model.Stats
{
    public class FileCoverage : IFileCoverage
    {
        public FileCoverage(string fileName,
                            IEnumerable<(int lineNumber, int hitCount)> lineCoverages)
        {
            FileName = fileName;
            LineCoverages = lineCoverages.Select(tuple => new LineCoverage(tuple.lineNumber, tuple.hitCount));
        }

        public string FileName { get; }

        public IEnumerable<ILineCoverage> LineCoverages { get; }

        public override string ToString() =>
            $"{FileName} : {string.Join(" ", LineCoverages.Select(lc => lc.ToString()))}";
    }
}
