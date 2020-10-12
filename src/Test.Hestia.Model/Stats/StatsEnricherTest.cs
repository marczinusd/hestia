using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using Hestia.Model;
using Hestia.Model.Builders;
using Hestia.Model.Interfaces;
using Hestia.Model.Stats;
using Hestia.Model.Wrappers;
using LanguageExt;
using Moq;
using Test.Hestia.Utils.TestData;
using Xunit;

namespace Test.Hestia.Model.Stats
{
    public class StatsEnricherTest
    {
        private readonly IEnumerable<IFileCoverage> _coverages = new[] { new FileCoverage("bla.js", new[] { (1, 1) }) };

        public static TheoryData<string, string> FileEnricherInvalidInput =>
            new TheoryData<string, string>
            {
                { string.Empty, string.Empty }, { string.Empty, null }, { null, string.Empty }, { null, null }
            };

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
            providerFactoryMock.Setup(mock => mock.CreateProviderForFile(It.IsAny<string>()))
                               .Returns(coverageProviderMock.Object);
            var enricher = fixture.Create<StatsEnricher>();
            var snapshotToEnrich = new RepositorySnapshot(string.Empty,
                                                          new List<IFile>(),
                                                          "coverage.json",
                                                          Option<string>.None,
                                                          Option<DateTime>.None,
                                                          string.Empty,
                                                          string.Empty);

            var enrichedSnapshot = enricher.EnrichWithCoverage(snapshotToEnrich);

            enrichedSnapshot.Should()
                            .NotBeSameAs(snapshotToEnrich);
        }

        [Fact]
        public void StatsEnricherThrowsExceptionOnCoverageEnrichIfCoveragePathIsNone()
        {
            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization { ConfigureMembers = true });
            var snapshot = new RepositorySnapshot(string.Empty,
                                                  new List<IFile>(),
                                                  Option<string>.None,
                                                  "hash",
                                                  Option<DateTime>.None,
                                                  string.Empty,
                                                  string.Empty);
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
                                                                   Mock.Of<IPathValidator>()),
                                                               GitStatGranularity.Full);

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

        [Fact]
        public void StatsEnricherEnrichesAllFilesInRepositorySnapshotWithCoverageStats()
        {
            var lineCoverages = new List<(int lineNumber, int hitCount)> { (1, 1), (5, 1) };
            var fixture = new Fixture();
            var coverageFactory = new Mock<ICoverageProviderFactory>();
            var coverageProvider = new Mock<ICoverageProvider>();
            coverageFactory.Setup(mock => mock.CreateProviderForFile(It.IsAny<string>()))
                           .Returns(coverageProvider.Object);
            coverageProvider.Setup(mock => mock.ParseFileCoveragesFromFilePath(It.IsAny<string>()))
                            .Returns(new[]
                            {
                                new FileCoverage(Path.GetFileName(MockRepo.FirstIncludedFilePath) ?? string.Empty,
                                                 lineCoverages)
                            });
            var ioMock = MockRepo.CreateDiskIOWrapperMock();
            fixture.Register(() => ioMock.Object);
            fixture.Register(() => coverageFactory.Object);
            fixture.Customize(new AutoMoqCustomization { ConfigureMembers = true });
            var enricher = fixture.Create<StatsEnricher>();

            var enrichedSnapshot = enricher.EnrichWithCoverage(MockRepo.CreateSnapshot(new[] { ".cs" },
                                                                   "lcov.info",
                                                                   ioMock.Object,
                                                                   Mock.Of<IPathValidator>()));

            enrichedSnapshot.Files
                            .Should()
                            .HaveCount(2);
            var firstCovStats = enrichedSnapshot.Files[0]
                                                .CoverageStats
                                                .Match(x => x,
                                                       () => MockRepo.DefaultCoverage);
            var secondCovStats = enrichedSnapshot.Files[1]
                                                 .CoverageStats.Match(x => x, () => MockRepo.DefaultCoverage);
            enrichedSnapshot.Files[0]
                            .Content.Should()
                            .HaveCount(5);
            enrichedSnapshot.Files[0]
                            .Content
                            .Where(x => x.LineCoverageStats.IsSome)
                            .Should()
                            .HaveCount(2);
            enrichedSnapshot.Files[1]
                            .Content.Should()
                            .HaveCount(3);
            firstCovStats.Coverage.LineCoverages
                         .Should()
                         .BeEquivalentTo(lineCoverages.Select(l => new LineCoverage(l.lineNumber, l.hitCount)));
            secondCovStats.Coverage.LineCoverages
                          .Should()
                          .BeEmpty();
        }

        [Fact]
        public void OptionIsNoneShouldBeThrownIfInvalidCoveragePathWasProvided()
        {
            var lineCoverages = new List<(int lineNumber, int hitCount)> { (1, 1), (5, 1) };
            var fixture = new Fixture();
            var coverageFactory = new Mock<ICoverageProviderFactory>();
            var coverageProvider = new Mock<ICoverageProvider>();
            var converter = new Mock<ICoverageReportConverter>();
            coverageFactory.Setup(mock => mock.CreateProviderForFile(It.IsAny<string>()))
                           .Returns(coverageProvider.Object);
            coverageProvider.Setup(mock => mock.ParseFileCoveragesFromFilePath(It.IsAny<string>()))
                            .Returns(new[]
                            {
                                new FileCoverage(Path.GetFileName(MockRepo.FirstIncludedFilePath) ?? string.Empty,
                                                 lineCoverages)
                            });
            converter.Setup(mock => mock.Convert(It.IsAny<string>(), It.IsAny<string>()))
                     .Returns(Option<string>.None);
            var ioMock = MockRepo.CreateDiskIOWrapperMock();
            fixture.Register(() => ioMock.Object);
            fixture.Register(() => coverageFactory.Object);
            fixture.Register(() => converter.Object);
            fixture.Customize(new AutoMoqCustomization { ConfigureMembers = true });
            var enricher = fixture.Create<StatsEnricher>();

            Action act = () => enricher.EnrichWithCoverage(MockRepo.CreateSnapshot(new[] { ".cs" },
                                                               string.Empty,
                                                               ioMock.Object,
                                                               Mock.Of<IPathValidator>()));

            act.Should()
               .Throw<OptionIsNoneException>();
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

            var repo = enricher.Enrich(new Repository("bla",
                                                      Option<IRepositorySnapshot[]>.None,
                                                      Option<string>.None,
                                                      Option<string>.None),
                                       args);

            var snapshots = repo.Snapshots.Match(x => x, () => Array.Empty<IRepositorySnapshot>());

            // verify behavior
            gitCommandsMock.Verify(mock => mock.Checkout(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(5));
            gitCommandsMock.Verify(mock => mock.DateOfLatestCommitOnBranch(It.IsAny<string>()), Times.Exactly(10)); // invoked redundantly due to how snapshots are enriched
            gitCommandsMock.Verify(mock => mock.GetHashForNthCommit(It.IsAny<string>(), It.IsAny<int>()),
                                   Times.Exactly(5));
            executorMock.Verify(mock => mock.Execute("dotnet cover", It.IsAny<string>(), It.IsAny<string>()),
                                Times.Exactly(5));

            // verify results
            snapshots.Should()
                     .HaveCount(5);
            snapshots.Select(s => s.AtHash.Match(x => x, string.Empty))
                     .Should()
                     .BeEquivalentTo("1",
                                     "13",
                                     "25",
                                     "37",
                                     "50");
        }

        [Fact]
        public void IfInvalidCoveragePathIsProvidedItShouldNotBeConverted()
        {
            var args = new RepositoryStatsEnricherArguments("bla",
                                                            "src",
                                                            new[] { ".cs" },
                                                            "dotnet cover",
                                                            string.Empty,
                                                            1,
                                                            50,
                                                            5);
            var fixture = new Fixture();
            var ioMock = MockRepo.CreateDiskIOWrapperMock();
            var gitCommandsMock = MockRepo.CreateGitCommandsMock();
            var executorMock = new Mock<ICommandLineExecutor>();
            var converter = new Mock<ICoverageReportConverter>();
            converter.Setup(mock => mock.Convert(It.IsAny<string>(), It.IsAny<string>()))
                     .Returns(Option<string>.None);
            gitCommandsMock.Setup(mock => mock.GetHashForNthCommit(It.IsAny<string>(), It.IsAny<int>()))
                           .Returns<string, int>((_, i) => i.ToString());
            fixture.Register(() => ioMock.Object);
            fixture.Register(() => gitCommandsMock.Object);
            fixture.Register(() => executorMock.Object);
            fixture.Register(() => converter.Object);
            fixture.Customize(new AutoMoqCustomization { ConfigureMembers = true });
            var enricher = fixture.Create<StatsEnricher>();

            var repo = enricher.Enrich(new Repository("bla",
                                                      Option<IRepositorySnapshot[]>.None,
                                                      Option<string>.None,
                                                      Option<string>.None),
                                       args);

            var snapshots = repo.Snapshots.Match(x => x, Array.Empty<IRepositorySnapshot>);

            snapshots.First()
                     .PathToCoverageResultFile.Some(x => x)
                     .None(() => "none")
                     .Should()
                     .Be(string.Empty);
        }
    }
}
