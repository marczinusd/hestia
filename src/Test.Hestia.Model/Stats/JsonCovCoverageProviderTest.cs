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
            Mock<IDiskIOWrapper> ioWrapperMock = new Mock<IDiskIOWrapper>();
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
                .BeEquivalentTo(new[]
                {
                    (15, 0),
                    (17, 2),
                    (19, 5),
                    (21, 7),
                    (7, 5),
                    (8, 5),
                    (9, 5),
                    (10, 5),
                    (11, 5),
                    (12, 5),
                    (13, 5),
                });
        }
    }
}
