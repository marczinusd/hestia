using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hestia.Model.Stats
{
    public interface ICoverageProvider
    {
        IEnumerable<FileCoverage> ParseFileCoveragesFromFilePath(string filePath);

        Task<IEnumerable<FileCoverage>> ParseFileCoveragesFromFilePathAsync(string filePath);
    }
}
