using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using Hestia.ConsoleRunner;
using Hestia.DAL.Interfaces;
using Hestia.Model;
using Hestia.Model.Builders;
using Hestia.Model.Interfaces;
using Hestia.Model.Stats;
using Hestia.Model.Wrappers;
using LanguageExt;
using Moq;
using Serilog;
using Test.Hestia.Utils.TestData;
using Xunit;
using static LanguageExt.Prelude;

namespace Test.Hestia.ConsoleRunner
{
    public class RunnerTest
    {
        private static readonly Func<RunnerConfig> ConfigFactory = () => new RunnerConfig
        {
            FileExtensions = new List<string> { ".cs" },
            IgnorePatterns = new List<string> { "(.*)Test.cs" },
            RepoRoot = "path",
            StatGranularity = "file",
            CoverageReportLocation = "path/coverage",
            SourceRelativePath = "src",
            SqliteDbLocation = "somePath",
            SqliteDbName = "some.db"
        };

        private static readonly Func<IRepositorySnapshot> Snapshot = () =>
        {
            var snapshot = MockRepo.CreateSnapshot(new[] { ".cs" },
                                                   "path",
                                                   Mock.Of<IDiskIOWrapper>(),
                                                   Mock.Of<IPathValidator>());
            var files = new List<IFile>(MockRepo.CreateFiles(4));
            files.Add(new File("BlablaTest",
                               ".cs",
                               "somepath",
                               new List<ISourceLine>(),
                               None,
                               None));

            return snapshot.With(files: files);
        };

        [Fact]
        public void BuildFromConfigInvokesSnapshotBuilder()
        {
            var snapshotBuilder = new Mock<IRepositorySnapshotBuilderWrapper>();
            snapshotBuilder.Setup(mock => mock.Build(It.IsAny<RepositorySnapshotBuilderArguments>()))
                           .Returns(Snapshot());
            var runner = CreateInitialBuilder()
                         .With(snapshotBuilder.Object)
                         .Build();

            runner.BuildFromConfig(ConfigFactory());

            snapshotBuilder.Verify(mock => mock.Build(It.IsAny<RepositorySnapshotBuilderArguments>()), Times.Once);
        }

        [Fact]
        public void BuildFromConfigInvokesCoverageReportConverter()
        {
            var converter = new Mock<ICoverageReportConverter>();
            converter.Setup(mock => mock.Convert(It.IsAny<string>(), It.IsAny<string>()))
                     .Returns("newPath");
            var runner = CreateInitialBuilder()
                         .With(converter.Object)
                         .Build();

            runner.BuildFromConfig(ConfigFactory());

            converter.Verify(mock => mock.Convert(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void BuildFromConfigFiltersSnapshotFilesBasedOnIgnorePaths()
        {
            // create snapshot with n files
            // verify that files matching Test.cs are filtered out in the statsenricher call
        }

        [Fact]
        public void BuildFromConfigLogsWarningIfInvalidFilterPatternWasProvided()
        {
            var log = new Mock<ILogger>();
            var runner = CreateInitialBuilder()
                         .With(log.Object)
                         .Build();
            var config = ConfigFactory();
            config.IgnorePatterns.Add("**.cs");

            runner.BuildFromConfig(config);

            log.Verify(mock => mock.Warning(It.Is<string>(s => s.Contains("is invalid"))), Times.Exactly(5));
        }

        [Fact]
        public void BuildFromConfigLogsWarningIfGranularityWasProvided()
        {
            var log = new Mock<ILogger>();
            var runner = CreateInitialBuilder()
                         .With(log.Object)
                         .Build();
            var config = ConfigFactory();
            config.StatGranularity = "bla";

            runner.BuildFromConfig(config);

            log.Verify(mock => mock.Warning(It.Is<string>(s => s.Contains("Could not parse git stat granularity"))),
                       Times.Once);
        }

        [Fact]
        public void BuildFromConfigInvokesCoverageEnrichmentOnSnapshot()
        {
            var enricher = new Mock<IStatsEnricher>();
            enricher.Setup(mock => mock.EnrichWithCoverage(It.IsAny<IRepositorySnapshot>()))
                    .Returns(Snapshot());
            enricher.Setup(mock => mock.EnrichWithGitStats(It.IsAny<IRepositorySnapshot>(),
                                                           It.IsAny<GitStatGranularity>(),
                                                           It.IsAny<Option<ISubject<int>>>()))
                    .Returns(Snapshot());
            var runner = CreateInitialBuilder()
                         .With(enricher.Object)
                         .Build();

            runner.BuildFromConfig(ConfigFactory());

            enricher.Verify(mock => mock.EnrichWithCoverage(It.IsAny<RepositorySnapshot>()), Times.Once);
        }

        [Fact]
        public void BuildFromConfigInvokesGitStatsEnrichmentOnSnapshot()
        {
            var enricher = new Mock<IStatsEnricher>();
            enricher.Setup(mock => mock.EnrichWithCoverage(It.IsAny<IRepositorySnapshot>()))
                    .Returns(Snapshot());
            enricher.Setup(mock => mock.EnrichWithGitStats(It.IsAny<IRepositorySnapshot>(),
                                                           It.IsAny<GitStatGranularity>(),
                                                           It.IsAny<Option<ISubject<int>>>()))
                    .Returns(Snapshot());
            var runner = CreateInitialBuilder()
                         .With(enricher.Object)
                         .Build();

            runner.BuildFromConfig(ConfigFactory());

            enricher.Verify(mock => mock.EnrichWithGitStats(It.IsAny<RepositorySnapshot>(),
                                                            It.IsAny<GitStatGranularity>(),
                                                            It.IsAny<Option<ISubject<int>>>()),
                            Times.Once);
        }

        [Fact]
        public void BuildFromConfigInvokesSnapshotPersistenceIfNotDryRun()
        {
            var persistence = new Mock<ISnapshotPersistence>();
            var runner = CreateInitialBuilder()
                         .With(persistence.Object)
                         .Build();

            runner.BuildFromConfig(ConfigFactory());

            persistence.Verify(mock => mock.InsertSnapshotSync(It.Is<IRepositorySnapshot>(_ => true)), Times.Once);
        }

        [Fact]
        public void BuildFromConfigDoesNotPersistOnDryRun()
        {
            var persistence = new Mock<ISnapshotPersistence>();
            var runner = CreateInitialBuilder()
                         .With(persistence.Object)
                         .Build();

            runner.BuildFromConfig(ConfigFactory(), true);

            persistence.Verify(mock => mock.InsertSnapshotSync(It.IsAny<IRepositorySnapshot>()), Times.Never);
        }

        private static RunnerBuilder CreateInitialBuilder()
        {
            var snapshotBuilder = new Mock<IRepositorySnapshotBuilderWrapper>();
            snapshotBuilder.Setup(mock => mock.Build(It.IsAny<RepositorySnapshotBuilderArguments>()))
                           .Returns(Snapshot());
            var converter = new Mock<ICoverageReportConverter>();
            converter.Setup(mock => mock.Convert(It.IsAny<string>(), It.IsAny<string>()))
                     .Returns("newPath");
            var enricher = new Mock<IStatsEnricher>();
            enricher.Setup(mock => mock.EnrichWithCoverage(It.IsAny<IRepositorySnapshot>()))
                    .Returns(Snapshot());
            enricher.Setup(mock => mock.EnrichWithGitStats(It.IsAny<IRepositorySnapshot>(),
                                                           It.IsAny<GitStatGranularity>(),
                                                           It.IsAny<Option<ISubject<int>>>()))
                    .Returns(Snapshot());

            return new RunnerBuilder().With(snapshotBuilder.Object)
                                      .With(converter.Object)
                                      .With(enricher.Object);
        }
    }
}
