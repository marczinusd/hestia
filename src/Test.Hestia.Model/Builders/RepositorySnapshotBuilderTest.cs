using System;
using System.IO;
using FluentAssertions;
using Hestia.Model.Builders;
using Hestia.Model.Wrappers;
using Moq;
using Xunit;

namespace Test.Hestia.Model.Builders
{
    public class RepositorySnapshotBuilderTest
    {
        private static readonly string DirPath = Path.Join("C:", "temp");
        private static readonly string[] RootDirs = { Path.Join(DirPath, "firstDir"), Path.Join(DirPath, "secondDir") };
        private static readonly string[] RootFiles = { Path.Join(DirPath, "package.json"), };

        [Fact]
        public void RepositorySnapshotBuilderBuildsExpectedStructureFromArguments()
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
                     .Returns(new[] { Path.Join(DirPath, "bla2.cs") });
            var args = new RepositorySnapshotBuilderArguments(1,
                                                              DirPath,
                                                              string.Empty,
                                                              new[] { ".cs" },
                                                              "lcov.info",
                                                              "hash",
                                                              default(DateTime),
                                                              ioWrapper.Object,
                                                              Mock.Of<IPathValidator>());

            var snapshot = args.Build();

            snapshot.Files
                    .Should()
                    .NotBeNull();
            snapshot.AtHash.Match(x => x, () => string.Empty)
                    .Should()
                    .Be("hash");
            snapshot.SnapshotId
                    .Should()
                    .Be(1);
            snapshot.CommitCreationDate.Match(x => x, DateTime.Today)
                    .Should()
                    .Be(default);
        }
    }
}
