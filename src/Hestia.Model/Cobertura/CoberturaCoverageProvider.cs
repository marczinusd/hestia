using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Hestia.Model.Extensions;
using Hestia.Model.Stats;
using Hestia.Model.Wrappers;

namespace Hestia.Model.Cobertura
{
    public class CoberturaCoverageProvider : ICoverageProvider
    {
        private readonly IFileStreamWrapper _fileStreamWrapper;

        public CoberturaCoverageProvider(IFileStreamWrapper fileStreamWrapper)
        {
            _fileStreamWrapper = fileStreamWrapper;
        }

        public IEnumerable<FileCoverage> ParseFileCoveragesFromFilePath(string filePath)
        {
            var rawCoverage = _fileStreamWrapper.Deserialize<Coverage>(filePath, FileMode.Open);

            return rawCoverage.Packages
                              .SelectMany(p => p.Classes)
                              .Select(c => new FileCoverage(c.Filename,
                                                            c.Lines.Select(l => (l.Number, l.Hits))))
                              .DistinctBy(c => c.FileName);
        }

        public Task<IEnumerable<FileCoverage>> ParseFileCoveragesFromFilePathAsync(string filePath) =>
            Task.Run(() => ParseFileCoveragesFromFilePath(filePath));
    }
}
