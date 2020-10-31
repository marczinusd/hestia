using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using Hestia.ConsoleRunner;
using Hestia.DAL.Interfaces;
using Hestia.Model;
using Hestia.Model.Builders;
using Hestia.Model.Interfaces;
using Hestia.Model.Stats;
using LanguageExt;
using Moq;
using Serilog;
using Xunit;

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

        [Fact]
        public void BuildFromConfigInvokesSnapshotBuilder()
        {
            var snapshotBuilder = new Mock<IRepositorySnapshotBuilderWrapper>();
            var runner = new RunnerBuilder().With(snapshotBuilder.Object)
                                            .Build();

            runner.BuildFromConfig(ConfigFactory());

            snapshotBuilder.Verify(mock => mock.Build(It.IsAny<RepositorySnapshotBuilderArguments>()), Times.Once);
        }

        [Fact]
        public void BuildFromConfigInvokesCoverageReportConverter()
        {
            var converter = new Mock<ICoverageReportConverter>();
            var runner = new RunnerBuilder().With(converter.Object)
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
            var runner = new RunnerBuilder().With(log.Object)
                                            .Build();

            runner.BuildFromConfig(ConfigFactory());

            log.Verify(mock => mock.Warning(It.Is<string>(s => s.Contains("is invalid"))), Times.Once);
        }

        [Fact]
        public void BuildFromConfigLogsWarningIfGranularityWasProvided()
        {
            var log = new Mock<ILogger>();
            var runner = new RunnerBuilder().With(log.Object)
                                            .Build();

            runner.BuildFromConfig(ConfigFactory());

            log.Verify(mock => mock.Warning(It.Is<string>(s => s.Contains("Could not parse git stat granularity"))),
                       Times.Once);
        }

        [Fact]
        public void BuildFromConfigInvokesCoverageEnrichmentOnSnapshot()
        {
            var enricher = new Mock<StatsEnricher>();
            var runner = new RunnerBuilder().With(enricher.Object)
                                            .Build();

            runner.BuildFromConfig(ConfigFactory());

            enricher.Verify(mock => mock.EnrichWithCoverage(It.IsAny<RepositorySnapshot>()), Times.Once);
        }

        [Fact]
        public void BuildFromConfigInvokesGitStatsEnrichmentOnSnapshot()
        {
            var enricher = new Mock<StatsEnricher>();
            var runner = new RunnerBuilder().With(enricher.Object)
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
            var runner = new RunnerBuilder().With(persistence.Object)
                                            .Build();

            runner.BuildFromConfig(ConfigFactory());

            persistence.Verify(mock => mock.InsertSnapshot(It.IsAny<IRepositorySnapshot>()), Times.Once);
        }

        [Fact]
        public void BuildFromConfigDoesNotPersistOnDryRun()
        {
            var persistence = new Mock<ISnapshotPersistence>();
            var runner = new RunnerBuilder().With(persistence.Object)
                                            .Build();

            runner.BuildFromConfig(ConfigFactory(), true);

            persistence.Verify(mock => mock.InsertSnapshot(It.IsAny<IRepositorySnapshot>()), Times.Never);
        }
    }
}
