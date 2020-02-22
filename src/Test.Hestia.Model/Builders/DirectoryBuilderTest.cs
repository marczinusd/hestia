using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using Hestia.Model.Builders;
using Hestia.Model.Wrappers;
using Moq;
using Xunit;

namespace Test.Hestia.Model.Builders
{
    public class DirectoryBuilderTest
    {
        private static string dirPath = Path.Join("C:", "temp");
        private static string[] rootDirs = { Path.Join(dirPath, "firstDir"), Path.Join(dirPath, "secondDir") };
        private static string[] rootFiles = { Path.Join(dirPath, "package.json"), };

        [Fact]
        public void DirectoryBuilderShouldBuildDirWithNoDirectoriesAndFilesForEmptyDirectory()
        {
            var ioWrapper = new Mock<IDiskIOWrapper>();
            ioWrapper.Setup(mock => mock.EnumerateAllDirectoriesForPath(dirPath))
                     .Returns(Enumerable.Empty<string>());

            var directory = DirectoryBuilder.BuildDirectoryFromDirectoryPath(dirPath,
                                                                             ioWrapper.Object,
                                                                             Mock.Of<IPathValidator>());

            directory.Directories
                     .Should()
                     .BeEmpty();
            directory.Files
                     .Should()
                     .BeEmpty();
        }

        [Fact]
        public void DirectoryBuilderShouldInvokePathValidator()
        {
            var ioWrapper = new Mock<IDiskIOWrapper>();
            ioWrapper.Setup(mock => mock.EnumerateAllDirectoriesForPath(dirPath))
                     .Returns(Enumerable.Empty<string>());

            var validator = new Mock<IPathValidator>();
            DirectoryBuilder.BuildDirectoryFromDirectoryPath(dirPath,
                                                             ioWrapper.Object,
                                                             validator.Object);

            validator.Verify(mock => mock.ValidateDirectoryPath(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void DirectoryBuilderShouldBuildExpectedStructureForExistingPaths()
        {
            var ioWrapper = new Mock<IDiskIOWrapper>();
            ioWrapper.Setup(mock => mock.EnumerateAllDirectoriesForPath(dirPath))
                     .Returns(rootDirs);
            ioWrapper.Setup(mock => mock.EnumerateAllFilesForPath(dirPath))
                     .Returns(rootFiles);
            ioWrapper.Setup(mock => mock.EnumerateAllDirectoriesForPath(rootDirs[0]))
                     .Returns(Array.Empty<string>());
            ioWrapper.Setup(mock => mock.EnumerateAllDirectoriesForPath(rootDirs[1]))
                     .Returns(Array.Empty<string>());
            ioWrapper.Setup(mock => mock.EnumerateAllFilesForPath(rootDirs[0]))
                     .Returns(new[] { Path.Join(dirPath, "bla.js") });
            ioWrapper.Setup(mock => mock.EnumerateAllFilesForPath(rootDirs[1]))
                     .Returns(new[] { Path.Join(dirPath, "bla2.js") });

            var result = DirectoryBuilder.BuildDirectoryFromDirectoryPath(dirPath,
                                                                          ioWrapper.Object,
                                                                          Mock.Of<IPathValidator>());

            result.Directories[0]
                  .Path.Should()
                  .BeEquivalentTo(rootDirs[0]);
            result.Directories[1]
                  .Path.Should()
                  .BeEquivalentTo(rootDirs[1]);
            result.Directories[0]
                  .Files.Should()
                  .HaveCount(1);
            result.Directories[0]
                  .Files[0]
                  .Filename.Should()
                  .Be("bla.js");
            result.Directories[1]
                  .Files.Should()
                  .HaveCount(1);
            result.Directories[1]
                  .Files[0]
                  .Filename.Should()
                  .Be("bla2.js");
        }
    }
}
