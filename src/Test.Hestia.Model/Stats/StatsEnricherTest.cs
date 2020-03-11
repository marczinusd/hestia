using System;
using System.Collections.Generic;
using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using Hestia.Model;
using Hestia.Model.Builders;
using Hestia.Model.Stats;
using Hestia.Model.Wrappers;
using LanguageExt;
using Microsoft.Extensions.Logging;
using Moq;
using Test.Hestia.Model.TestData;
using Xunit;
using File = Hestia.Model.File;

namespace Test.Hestia.Model.Stats
{
    public class StatsEnricherTest
    {
        private readonly IEnumerable<FileCoverage> _coverages = new[] { new FileCoverage("bla.js", new[] { (1, 1) }) };

        [Fact]
        public void StatsEnricherEnrichesSnapshotWithCoverageCorrectlyTest()
        {
            var ioWrapperMock = MockRepo.CreateDiskIOWrapperMock();
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
                                                          new List<File>(),
                                                          "coverage.json",
                                                          Option<string>.None,
                                                          Option<DateTime>.None);

            var enrichedSnapshot = enricher.EnrichWithCoverage(snapshotToEnrich);

            enrichedSnapshot.Should()
                            .NotBeSameAs(snapshotToEnrich);
        }

        [Fact]
        public void StatsEnricherThrowsExceptionOnCoverageEnrichIfCoveragePathIsNone()
        {
            var snapshot = new RepositorySnapshot(1,
                                                  new List<File>(),
                                                  Option<string>.None,
                                                  "hash",
                                                  Option<DateTime>.None);
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

        // TODO
        [Fact]
        public void StatsEnricherEnrichesAllFilesInRepositorySnapshotWithGitStats()
        {
            var fixture = new Fixture();
            var ioMock = MockRepo.CreateDiskIOWrapperMock();
            fixture.Register(() => ioMock.Object);
            fixture.Customize(new AutoMoqCustomization { ConfigureMembers = true });
            var enricher = fixture.Create<StatsEnricher>();

            var enrichedSnapshot = enricher.EnrichWithGitStats(MockRepo.CreateSnapshot(new[] { ".cs" },
                                                                                       "lcov.info",
                                                                                       ioMock.Object,
                                                                                       Mock.Of<IPathValidator>()));

            enrichedSnapshot.Files
                            .Should()
                            .HaveCount(2);
        }

        // TODO
        [Fact]
        public void StatsEnricherEnrichesAllFilesInRepositorySnapshotWithCoverageStats()
        {
            var fixture = new Fixture();
            var ioMock = MockRepo.CreateDiskIOWrapperMock();
            fixture.Register(() => ioMock.Object);
            fixture.Customize(new AutoMoqCustomization { ConfigureMembers = true });
            var enricher = fixture.Create<StatsEnricher>();

            var enrichedSnapshot = enricher.EnrichWithGitStats(MockRepo.CreateSnapshot(new[] { ".cs" },
                                                                                       "lcov.info",
                                                                                       ioMock.Object,
                                                                                       Mock.Of<IPathValidator>()));

            enrichedSnapshot.Files
                            .Should()
                            .HaveCount(2);
        }

        [Fact]
        public void EnrichOnStatsEnricherEnrichesAllFilesInRepositorySnapshotWithAllStats()
        {
        }

        [Fact]
        public void EnrichOnRepositoryCreatesEnrichedSnapshotsCorrectly()
        {
        }
    }
}
