using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Hestia.Model.Extensions;
using Hestia.Model.Stats;

namespace Hestia.Model.Cobertura
{
    public class CoberturaCoverageProvider : ICoverageProvider
    {
        private static readonly XmlSerializer Serializer = new XmlSerializer(typeof(Coverage));

        public IEnumerable<FileCoverage> ParseFileCoveragesFromFilePath(string filePath)
        {
            using Stream reader = new FileStream(filePath, FileMode.Open);
            var rawCoverage = (Coverage)Serializer.Deserialize(reader);

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
