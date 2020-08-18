using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Hestia.Model.Cobertura;
using Hestia.Model.Wrappers;
using Moq;
using Test.Hestia.Model.Resources;
using Test.Hestia.Utils;
using Xunit;

namespace Test.Hestia.Model.Cobertura
{
    public class CoberturaCoverageProviderTest
    {
        private const string Path = "somePath";

        [Fact]
        public async Task CoberturaXmlDeserializesCorrectly()
        {
            var coverage =
                Helpers.LoadAndDeserializeXmlResource<Coverage>(Paths.CoberturaXml,
                                                                typeof(CoberturaCoverageProviderTest).Assembly);
            var fileStreamWrapperMock = new Mock<IXmlFileSerializationWrapper>();
            fileStreamWrapperMock.Setup(mock => mock.Deserialize<Coverage>(It.IsAny<string>(), FileMode.Open))
                                 .Returns(coverage);
            var provider = new CoberturaCoverageProvider(fileStreamWrapperMock.Object);

            var result = (await provider.ParseFileCoveragesFromFilePathAsync(Path)).ToList();

            result.Count
                  .Should()
                  .Be(1);
            result.First()
                  .LineCoverages
                  .Should()
                  .HaveCount(12);
        }
    }
}
