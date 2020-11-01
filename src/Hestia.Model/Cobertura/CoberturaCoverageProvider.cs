using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Hestia.Model.Extensions;
using Hestia.Model.Interfaces;
using Hestia.Model.Stats;
using Hestia.Model.Wrappers;

namespace Hestia.Model.Cobertura
{
    public class CoberturaCoverageProvider : ICoverageProvider
    {
        private readonly IXmlFileSerializationWrapper _xmlFileSerializationWrapper;

        public CoberturaCoverageProvider(IXmlFileSerializationWrapper xmlFileSerializationWrapper) =>
            _xmlFileSerializationWrapper = xmlFileSerializationWrapper;

        public IEnumerable<IFileCoverage> ParseFileCoveragesFromFilePath(string filePath)
        {
            var rawCoverage = _xmlFileSerializationWrapper.Deserialize<Coverage>(filePath, FileMode.Open);

            return rawCoverage.Packages
                              .SelectMany(p => p.Classes)
                              .Select(c => new FileCoverage(c.Filename,
                                                            c.Lines.Select(l => (l.Number, l.Hits, l.Branch,
                                                                                           l.ConditionCoverage ??
                                                                                           string.Empty))))
                              .DistinctBy(c => c.FileName);
        }

        public Task<IEnumerable<IFileCoverage>> ParseFileCoveragesFromFilePathAsync(string filePath) =>
            Task.Run(() => ParseFileCoveragesFromFilePath(filePath));
    }
}
