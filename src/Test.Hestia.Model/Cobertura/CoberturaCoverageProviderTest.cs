﻿using System.IO;
using System.Linq;
using FluentAssertions;
using Hestia.Model.Cobertura;
using Hestia.Model.Wrappers;
using Moq;
using Test.Hestia.Utils;
using Xunit;

namespace Test.Hestia.Model.Cobertura
{
    public class CoberturaCoverageProviderTest
    {
        private const string Path = "somePath";

        [Fact]
        public void CoberturaXmlDeserializesCorrectly()
        {
            var coverage = Helpers.LoadAndDeserializeXmlResource<Coverage>(Resources.Paths.CoberturaXml, typeof(CoberturaCoverageProviderTest).Assembly);
            var fileStreamWrapperMock = new Mock<IFileStreamWrapper>();
            fileStreamWrapperMock.Setup(mock => mock.Deserialize<Coverage>(It.IsAny<string>(), FileMode.Open))
                                 .Returns(coverage);
            var provider = new CoberturaCoverageProvider(fileStreamWrapperMock.Object);

            var result = provider.ParseFileCoveragesFromFilePath(Path).ToList();

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
