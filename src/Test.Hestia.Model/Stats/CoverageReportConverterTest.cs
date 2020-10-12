using System;
using System.IO;
using FluentAssertions;
using Hestia.Model.Stats;
using Hestia.Model.Wrappers;
using Moq;
using Xunit;

namespace Test.Hestia.Model.Stats
{
    public class CoverageReportConverterTest
    {
        [Fact]
        public void CoverageReportConverterShouldThrowFileNotFoundExceptionIfInputFileWasNotFound()
        {
            const string path = "invalidPath";
            var ioMock = new Mock<IDiskIOWrapper>();
            var reportGeneratorMock = new Mock<IReportGeneratorWrapper>();
            ioMock.Setup(mock => mock.FileExists(path))
                  .Returns(false);
            var converter = new CoverageReportConverter(ioMock.Object, reportGeneratorMock.Object);

            Action act = () => converter.Convert(path, "any");

            act.Should()
               .Throw<FileNotFoundException>()
               .WithMessage($"*{path}*");
            ioMock.Verify(mock => mock.FileExists(path), Times.Once);
        }

        [Fact]
        public void CoverageReportGeneratorShouldInvokeGenerateOnReportGeneratorWrapper()
        {
            const string path = "somePath";
            const string outPath = "someLocation";
            var ioMock = new Mock<IDiskIOWrapper>();
            var reportGeneratorMock = new Mock<IReportGeneratorWrapper>();
            ioMock.Setup(mock => mock.FileExists(path))
                  .Returns(true);
            var converter = new CoverageReportConverter(ioMock.Object, reportGeneratorMock.Object);

            converter.Convert(path, outPath);

            reportGeneratorMock.Verify(mock => mock.Generate(path, outPath), Times.Once);
        }

        [Fact]
        public void IfReportConversionIsSuccessfulConvertShouldReturnFullPathToNewReport()
        {
            const string path = "somePath";
            const string outPath = "someLocation";
            var ioMock = new Mock<IDiskIOWrapper>();
            var reportGeneratorMock = new Mock<IReportGeneratorWrapper>();
            reportGeneratorMock.Setup(mock => mock.Generate(path, outPath))
                               .Returns(true);
            ioMock.Setup(mock => mock.FileExists(path))
                  .Returns(true);
            var converter = new CoverageReportConverter(ioMock.Object, reportGeneratorMock.Object);

            var result = converter.Convert(path, outPath);

            result.Match(x => x, () => string.Empty)
                  .Should()
                  .BeEquivalentTo(Path.Join(outPath, "Cobertura.xml"));
        }

        [Theory]
        [InlineData("somePath/coverage.json")]
        [InlineData("somePath/cobertura.xml")]
        public void ReturnOriginalFilePathIfNoConversionIsNecessary(string path)
        {
            const string outPath = "someLocation";
            var ioMock = new Mock<IDiskIOWrapper>();
            var reportGeneratorMock = new Mock<IReportGeneratorWrapper>();
            ioMock.Setup(mock => mock.FileExists(path))
                  .Returns(true);
            var converter = new CoverageReportConverter(ioMock.Object, reportGeneratorMock.Object);

            var result = converter.Convert(path, outPath);

            result.Match(x => x, string.Empty)
                  .Should()
                  .Be(path);
            reportGeneratorMock.Verify(mock => mock.Generate(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void IfReportGeneratorFailsConvertShouldReturnNone()
        {
            const string path = "somePath";
            const string outPath = "someLocation";
            var ioMock = new Mock<IDiskIOWrapper>();
            var reportGeneratorMock = new Mock<IReportGeneratorWrapper>();
            reportGeneratorMock.Setup(mock => mock.Generate(path, outPath))
                               .Returns(false);
            ioMock.Setup(mock => mock.FileExists(path))
                  .Returns(true);
            var converter = new CoverageReportConverter(ioMock.Object, reportGeneratorMock.Object);

            var result = converter.Convert(path, outPath);

            result.IsNone
                  .Should()
                  .BeTrue();
        }
    }
}
