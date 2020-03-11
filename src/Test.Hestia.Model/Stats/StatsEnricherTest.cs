using System;
using System.Collections.Generic;
using System.IO;
using FluentAssertions;
using Hestia.Model;
using Hestia.Model.Stats;
using Hestia.Model.Wrappers;
using LanguageExt;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using File = Hestia.Model.File;

namespace Test.Hestia.Model.Stats
{
    public class StatsEnricherTest
    {
        private static readonly string DirPath = Path.Join("C:", "temp");
        private static readonly string[] RootDirs = { Path.Join(DirPath, "firstDir"), Path.Join(DirPath, "secondDir") };
        private static readonly string[] RootFiles = { Path.Join(DirPath, "package.json"), };

        private readonly IEnumerable<FileCoverage> _coverages = new[] { new FileCoverage("bla.js", new[] { (1, 1) }) };

        [Fact]
        public void StatsEnricherEnrichesSnapshotWithCoverageCorrectlyTest()
        {
            var ioWrapperMock = CreateDiskIOWrapperMock();
            var gitMock = new Mock<IGitCommands>();
            var executorMock = new Mock<ICommandLineExecutor>();
            var providerFactoryMock = new Mock<ICoverageProviderFactory>();
            var coverageProviderMock = new Mock<ICoverageProvider>();
            coverageProviderMock.Setup(mock => mock.ParseFileCoveragesFromFilePath(It.IsAny<string>()))
                                .Returns(_coverages);
            providerFactoryMock.Setup(mock => mock.CreateProviderForFile())
                               .Returns(coverageProviderMock.Object);
            var enricher = new StatsEnricher(ioWrapperMock.Object,
                                             gitMock.Object,
                                             Mock.Of<ILogger<IStatsEnricher>>(),
                                             executorMock.Object,
                                             providerFactoryMock.Object);
            var snapshotToEnrich = new RepositorySnapshot(1,
                                                          "coverage.json",
                                                          Option<string>.None,
                                                          Option<DateTime>.None,
                                                          new List<File>());

            var enrichedSnapshot = enricher.EnrichWithCoverage(snapshotToEnrich);

            enrichedSnapshot.Should()
                            .NotBeSameAs(snapshotToEnrich);
        }

        [Fact]
        public void StatsEnricherThrowsExceptionOnCoverageEnrichIfCoveragePathIsNone()
        {
            var snapshot = new RepositorySnapshot(1,
                                                  Option<string>.None,
                                                  "hash",
                                                  Option<DateTime>.None,
                                                  new List<File>());
            var enricher = new StatsEnricher(Mock.Of<IDiskIOWrapper>(),
                                             Mock.Of<IGitCommands>(),
                                             Mock.Of<ILogger<IStatsEnricher>>(),
                                             Mock.Of<ICommandLineExecutor>(),
                                             Mock.Of<ICoverageProviderFactory>());

            Action act = () => enricher.EnrichWithCoverage(snapshot);

            act.Should()
               .Throw<OptionIsNoneException>()
               .WithMessage("*PathToCoverageResultFile*");
        }

        private Mock<IDiskIOWrapper> CreateDiskIOWrapperMock()
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

            return ioWrapper;
        }
    }
}
