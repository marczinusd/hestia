using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hestia.Model.Interfaces;
using Hestia.Model.Wrappers;
using Newtonsoft.Json.Linq;

namespace Hestia.Model.Stats
{
    public class JsonCovCoverageProvider : ICoverageProvider
    {
        private readonly IDiskIOWrapper _diskIoWrapper;

        public JsonCovCoverageProvider(IDiskIOWrapper diskIoWrapper) => _diskIoWrapper = diskIoWrapper;

        // JSON structure: Assembly[] -> File[] -> Class[] -> Method[] -> Lines & Branches
        // See example json in Test.Hestia.Model.Resources -> coverage.json
        public IEnumerable<IFileCoverage> ParseFileCoveragesFromFilePath(string filePath) =>
            JObject
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

        public Task<IEnumerable<IFileCoverage>> ParseFileCoveragesFromFilePathAsync(string filePath)
            => Task.Run(() => ParseFileCoveragesFromFilePath(filePath));

        private static IEnumerable<(int lineNumber, int hitCount)> ParseLineCoverageJObject(
            JToken? token) =>
            token!.Children<JProperty>()
                  .Select(prop => (int.Parse(prop.Name),
                                   int.Parse(prop.Value.ToString())));
    }
}
