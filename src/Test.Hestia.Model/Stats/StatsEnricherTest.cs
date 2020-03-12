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
            var fixture = new Fixture();
            var providerFactoryMock = new Mock<ICoverageProviderFactory>();
            var coverageProviderMock = new Mock<ICoverageProvider>();
            fixture.Customize(new AutoMoqCustomization { ConfigureMembers = true });
            fixture.Register(() => MockRepo.CreateDiskIOWrapperMock()
                                           .Object);
            fixture.Register(() => providerFactoryMock.Object);
            fixture.Register(() => coverageProviderMock.Object);
            coverageProviderMock.Setup(mock => mock.ParseFileCoveragesFromFilePath(It.IsAny<string>()))
                                .Returns(_coverages);
            providerFactoryMock.Setup(mock => mock.CreateProviderForFile())
                               .Returns(coverageProviderMock.Object);
            var enricher = fixture.Create<StatsEnricher>();
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
            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization { ConfigureMembers = true });
            var snapshot = new RepositorySnapshot(1,
                                                  new List<File>(),
                                                  Option<string>.None,
                                                  "hash",
                                                  Option<DateTime>.None);
            var enricher = fixture.Create<StatsEnricher>();

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
            var args = new RepositoryStatsEnricherArguments("bla",
                                                            "src",
                                                            new[] { ".cs" },
                                                            "dotnet cover",
                                                            "lcov.info",
                                                            1,
                                                            50,
                                                            5);
            var fixture = new Fixture();
            var ioMock = MockRepo.CreateDiskIOWrapperMock();
            var gitCommandsMock = MockRepo.CreateGitCommandsMock();
            var executorMock = new Mock<ICommandLineExecutor>();
            gitCommandsMock.Setup(mock => mock.GetHashForNthCommit(It.IsAny<string>(), It.IsAny<int>()))
                           .Returns<string, int>((_, i) => i.ToString());
            fixture.Register(() => ioMock.Object);
            fixture.Register(() => gitCommandsMock.Object);
            fixture.Register(() => executorMock.Object);
            fixture.Customize(new AutoMoqCustomization { ConfigureMembers = true });
            var enricher = fixture.Create<StatsEnricher>();

            var repo = enricher.Enrich(new Repository(0,
                                                      "bla",
                                                      Option<RepositorySnapshot[]>.None,
                                                      Option<string>.None,
                                                      Option<string>.None),
                                       args);

            var snapshots = repo.Snapshots.Match(x => x, Array.Empty<RepositorySnapshot>());

            // verify behavior
            gitCommandsMock.Verify(mock => mock.Checkout(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(5));
            gitCommandsMock.Verify(mock => mock.DateOfLatestCommitOnBranch(It.IsAny<string>()), Times.Exactly(5));
            gitCommandsMock.Verify(mock => mock.GetHashForNthCommit(It.IsAny<string>(), It.IsAny<int>()),
                                   Times.Exactly(5));
            executorMock.Verify(mock => mock.Execute("dotnet cover", It.IsAny<string>(), It.IsAny<string>()),
                                Times.Exactly(5));

            // verify results
            snapshots.Should()
                     .HaveCount(5);
            snapshots.Select(s => s.SnapshotId)
                     .Should()
                     .BeEquivalentTo(new[] { 1, 2, 3, 4, 5 });
            snapshots.Select(s => s.AtHash.Match(x => x, string.Empty))
                     .Should()
                     .BeEquivalentTo(new[] { "1", "13", "25", "37", "50" });
        }
    }
}
