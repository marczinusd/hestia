using System.Collections.Generic;

namespace Hestia.Model.Interfaces
{
    public interface IFileCoverage
    {
        string FileName { get; }

        IEnumerable<ILineCoverage> LineCoverages { get; }
    }
}
