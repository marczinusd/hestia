using System;
using System.Collections.Generic;
using System.Linq;
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

        [Fact]
        public void StatsEnricherEnrichesAllFilesInRepositorySnapshotWithGitStats()
        {
            var fixture = new Fixture();
            var ioMock = MockRepo.CreateDiskIOWrapperMock();
            var gitCommandsMock = MockRepo.CreateGitCommandsMock();
            fixture.Register(() => ioMock.Object);
            fixture.Register(() => gitCommandsMock.Object);
            fixture.Customize(new AutoMoqCustomization { ConfigureMembers = true });
            var enricher = fixture.Create<StatsEnricher>();

            var enrichedSnapshot = enricher.EnrichWithGitStats(MockRepo.CreateSnapshot(new[] { ".cs" },
                                                                                       "lcov.info",
                                                                                       ioMock.Object,
                                                                                       Mock.Of<IPathValidator>()));

            enrichedSnapshot.Files
                            .Should()
                            .HaveCount(2);
            enrichedSnapshot.Files.Select(f => f.Content.Count)
                            .Should()
                            .AllBeEquivalentTo(3);
            enrichedSnapshot.Files[0]
                            .GitStats.Match(x => x, null as FileGitStats)
                            .LifetimeAuthors.Should()
                            .Be(MockRepo.FirstIncludedFileGitStats);
            enrichedSnapshot.Files[0]
                            .GitStats.Match(x => x, null as FileGitStats)
                            .LifetimeChanges.Should()
                            .Be(MockRepo.FirstIncludedFileGitStats);
            enrichedSnapshot.Files[1]
                            .GitStats.Match(x => x, null as FileGitStats)
                            .LifetimeAuthors.Should()
                            .Be(MockRepo.SecondIncludedFileGitStats);
            enrichedSnapshot.Files[1]
                            .GitStats.Match(x => x, null as FileGitStats)
                            .LifetimeChanges.Should()
                            .Be(MockRepo.SecondIncludedFileGitStats);
            enrichedSnapshot.Files[0]
                            .Content.Select(x => x.LineGitStats.Match(s => s, null as LineGitStats))
                            .All(s => s.NumberOfLifetimeAuthors == 2 && s.ModifiedInNumberOfCommits == 2)
                            .Should()
                            .BeTrue();
            enrichedSnapshot.Files[1]
                            .Content.Select(x => x.LineGitStats.Match(s => s, null as LineGitStats))
                            .All(s => s.NumberOfLifetimeAuthors == 2 && s.ModifiedInNumberOfCommits == 2)
                            .Should()
                            .BeTrue();
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

            var enrichedSnapshot = enricher.EnrichWithCoverage(MockRepo.CreateSnapshot(new[] { ".cs" },
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
