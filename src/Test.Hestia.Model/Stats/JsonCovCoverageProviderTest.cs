using System.Linq;
using FluentAssertions;
using Hestia.Model.Stats;
using Hestia.Model.Wrappers;
using Moq;
using Test.Hestia.Model.Utils;
using Xunit;

namespace Test.Hestia.Model.Stats
{
    public class JsonCovCoverageProviderTest
    {
        [Fact]
        public void JsonCovCoverageProviderParsesCoverageJsonAsExpected()
        {
            const string filePath = "somePath";
            var ioWrapperMock = new Mock<IDiskIOWrapper>();
            ioWrapperMock.Setup(mock => mock.ReadFileContent(filePath))
                         .Returns(() => Helpers.LoadResource("coverage.json",
                                                             typeof(JsonCovCoverageProviderTest).Assembly));
            var provider = new JsonCovCoverageProvider(ioWrapperMock.Object);

            var result = provider
                         .ParseFileCoveragesFromFilePathAsync(filePath)
                         .Result
                         .ToList();

            result.Should()
                  .HaveCount(21);
            result[0]
                  .FileName.Should()
                  .Contain("Directory.cs");
            result[0]
                .LineCoverages.Should()
                .BeEquivalentTo(new LineCoverage(15, 0),
                                new LineCoverage(17, 2),
                                new LineCoverage(19, 5),
                                new LineCoverage(21, 7),
                                new LineCoverage(7, 5),
                                new LineCoverage(8, 5),
                                new LineCoverage(9, 5),
                                new LineCoverage(10, 5),
                                new LineCoverage(11, 5),
                                new LineCoverage(12, 5),
                                new LineCoverage(13, 5));
        }
    }
}
