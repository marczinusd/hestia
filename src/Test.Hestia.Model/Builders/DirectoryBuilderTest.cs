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
        private static readonly string DirPath = Path.Join("C:", "temp");
        private static readonly string[] RootDirs = { Path.Join(DirPath, "firstDir"), Path.Join(DirPath, "secondDir") };
        private static readonly string[] RootFiles = { Path.Join(DirPath, "package.json"), };

        [Fact]
        public void DirectoryBuilderShouldBuildDirWithNoDirectoriesAndFilesForEmptyDirectory()
        {
            var ioWrapper = new Mock<IDiskIOWrapper>();
            ioWrapper.Setup(mock => mock.EnumerateAllDirectoriesForPath(DirPath))
                     .Returns(Enumerable.Empty<string>());

            var directory = DirectoryBuilder.BuildDirectoryFromDirectoryPath(DirPath,
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
        public void DirectoryBuilderShouldBuildExpectedStructureForExistingPaths()
        {
            var ioWrapper = new Mock<IDiskIOWrapper>();
            ioWrapper.Setup(mock => mock.EnumerateAllDirectoriesForPath(DirPath))
                     .Returns(RootDirs);
            ioWrapper.Setup(mock => mock.EnumerateAllFilesForPath(DirPath))
                     .Returns(RootFiles);
            ioWrapper.Setup(mock => mock.EnumerateAllDirectoriesForPath(RootDirs[0]))
                     .Returns(Array.Empty<string>());
            ioWrapper.Setup(mock => mock.EnumerateAllDirectoriesForPath(RootDirs[1]))
                     .Returns(Array.Empty<string>());
            ioWrapper.Setup(mock => mock.EnumerateAllFilesForPath(RootDirs[0]))
                     .Returns(new[] { Path.Join(DirPath, "bla.js") });
            ioWrapper.Setup(mock => mock.EnumerateAllFilesForPath(RootDirs[1]))
                     .Returns(new[] { Path.Join(DirPath, "bla2.js") });

            var result = DirectoryBuilder.BuildDirectoryFromDirectoryPath(DirPath,
                                                                          ioWrapper.Object,
                                                                          Mock.Of<IPathValidator>());

            result.Directories[0]
                  .Path.Should()
                  .BeEquivalentTo(RootDirs[0]);
            result.Directories[1]
                  .Path.Should()
                  .BeEquivalentTo(RootDirs[1]);
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

        [Fact]
        public void DirectoryBuilderShouldInvokePathValidator()
        {
            var ioWrapper = new Mock<IDiskIOWrapper>();
            ioWrapper.Setup(mock => mock.EnumerateAllDirectoriesForPath(DirPath))
                     .Returns(Enumerable.Empty<string>());

            var validator = new Mock<IPathValidator>();
            DirectoryBuilder.BuildDirectoryFromDirectoryPath(DirPath,
                                                             ioWrapper.Object,
                                                             validator.Object);

            validator.Verify(mock => mock.ValidateDirectoryPath(It.IsAny<string>()), Times.Once);
        }
    }
}
