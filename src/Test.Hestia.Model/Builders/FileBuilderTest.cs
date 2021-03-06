using System;
using System.IO;
using FluentAssertions;
using Hestia.Model.Builders;
using Hestia.Model.Wrappers;
using Moq;
using Xunit;

namespace Test.Hestia.Model.Builders
{
    public class FileBuilderTest
    {
        private static readonly string FilePath = Path.Join("C:",
                                                            "somedir",
                                                            "somesubdir",
                                                            "somefile.ext");

        private static readonly string[] FileContent = { "first line", "second line" };

        [Fact]
        public void FileBuilderShouldBuildFileObjectWithExpectedProperties()
        {
            var ioMock = new Mock<IDiskIOWrapper>();
            ioMock.Setup(mock => mock.ReadAllLinesFromFile(FilePath))
                  .Returns(FileContent);

            var result = FileBuilder.BuildFileFromPath(FilePath, ioMock.Object);

            result.Extension.Should()
                  .Be(".ext");
            result.Filename.Should()
                  .Be("somefile.ext");
            result.Path.Should()
                  .Be(Path.Join("C:", "somedir", "somesubdir"));
            result.Content[0]
                  .Text.Should()
                  .Be("first line");
            result.Content[1]
                  .Text.Should()
                  .Be("second line");
        }

        [Fact]
        public void FileBuilderShouldThrowExceptionWhenGivenANullParameter()
        {
            Action act = () => FileBuilder.BuildFileFromPath(null!, Mock.Of<IDiskIOWrapper>());

            act.Should()
               .Throw<FileBuilderException>();
        }
    }
}
