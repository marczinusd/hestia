using System.Collections.Generic;
using System.Threading.Tasks;
using Hestia.Model.Interfaces;

namespace Hestia.Model.Stats
{
    public interface ICoverageProvider
    {
        IEnumerable<IFileCoverage> ParseFileCoveragesFromFilePath(string filePath);

        Task<IEnumerable<IFileCoverage>> ParseFileCoveragesFromFilePathAsync(string filePath);
    }
}
