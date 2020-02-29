using System.Collections.Generic;

namespace Hestia.DAL.Entities
{
    public class FileCoverageEntity
    {
        public string FileName { get; set; }

        public IEnumerable<(int lineNumber, int hitCount)> LineCoverages { get; set; }
    }
}
