using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hestia.Model.Wrappers;
using Newtonsoft.Json.Linq;

namespace Hestia.Model.Stats
{
    public class JsonCovCoverageProvider : ICoverageProvider
    {
        private readonly IDiskIOWrapper _diskIoWrapper;

        public JsonCovCoverageProvider(IDiskIOWrapper diskIoWrapper)
        {
            _diskIoWrapper = diskIoWrapper;
        }

        public IEnumerable<FileCoverage> ParseFileCoveragesFromFilePath(string filePath)
        {
            // JSON structure: Assembly[] -> File[] -> Class[] -> Method[] -> Lines & Branches
            return JObject
                   .Parse(_diskIoWrapper.ReadFileContent(filePath))
                   .Children<JProperty>() // assemblies
                   .SelectMany(assembly =>
                                   assembly.Values()) // project to physical files
                   .OfType<JProperty>()
                   .SelectMany(f => f.Values()
                                     .SelectMany(c => c.Values())
                                     .OfType<JProperty>()
                                     .Select(method =>
                                                 (f.Name,
                                                  ParseLineCoverageJObject(method
                                                                           .Children()["Lines"]
                                                                           .First() as JObject))))
                   .GroupBy(x => x.Name, tuple => tuple) // project to (FileName, Lines[]) group
                   .Select(g => new
                               FileCoverage(g.Key, g.SelectMany(l => l.Item2)));
        }

        public Task<IEnumerable<FileCoverage>> ParseFileCoveragesFromFilePathAsync(string filePath)
            => Task.Run(() => ParseFileCoveragesFromFilePath(filePath));

        private static IEnumerable<(int lineNumber, int hitCount)> ParseLineCoverageJObject(
            JObject? jObject)
        {
            if (jObject == null)
            {
                return new[] { (-1, 0) };
            }

            return jObject.Children<JProperty>()
                          .Select(prop => (int.Parse(prop.Name),
                                           int.Parse(prop.Value.ToString())));
        }
    }
}
