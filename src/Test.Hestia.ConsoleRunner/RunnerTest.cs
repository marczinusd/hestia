using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
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
            SqliteDbName = "some.db",
            BuildCommands = new List<string>(),
            TestCommands = new List<string>()
        };

        private static readonly Func<IRepositorySnapshot> Snapshot = () =>
        {
            var snapshot = MockRepo.CreateSnapshot(new[] { ".cs" },
                                                   "path",
                                                   Mock.Of<IDiskIOWrapper>(),
                                                   Mock.Of<IPathValidator>());
            var files = new List<IFile>(MockRepo.CreateFiles(4));
            files.Add(new File("BlablaTest.cs",
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

            runner.BuildFromConfig(ConfigFactory(), false);

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

            runner.BuildFromConfig(ConfigFactory(), false);

            converter.Verify(mock => mock.Convert(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void BuildFromConfigFiltersSnapshotFilesBasedOnIgnorePaths()
        {
            var log = new Mock<ILogger>();
            var enricher = new Mock<IStatsEnricher>();
            enricher.Setup(mock => mock.EnrichWithCoverage(It.IsAny<IRepositorySnapshot>()))
                    .Returns<IRepositorySnapshot>(s => s);
            enricher.Setup(mock => mock.EnrichWithGitStats(It.IsAny<IRepositorySnapshot>(),
                                                           It.IsAny<GitStatGranularity>(),
                                                           It.IsAny<Option<ISubject<int>>>()))
                    .Returns<IRepositorySnapshot, GitStatGranularity, Option<ISubject<int>>>((s, _, __) => s);
            var runner = CreateInitialBuilder()
                         .With(log.Object)
                         .With(enricher.Object)
                         .Build();
            var config = ConfigFactory();
            config.IgnorePatterns.Add("(.*)Test.cs");

            runner.BuildFromConfig(config, false);

            enricher.Verify(mock => mock.EnrichWithGitStats(It.Is<IRepositorySnapshot>(s => s.Files.Count == 4),
                                                            It.IsAny<GitStatGranularity>(),
                                                            It.IsAny<Option<ISubject<int>>>()),
                            Times.Once);
            log.Verify(mock => mock.Debug(It.Is<string>(s => s.Contains("Filtering out"))), Times.Once);
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

            runner.BuildFromConfig(config, false);

            log.Verify(mock => mock.Warning(It.Is<string>(s => s.Contains("is invalid"))), Times.Exactly(4));
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

            runner.BuildFromConfig(config, false);

            log.Verify(mock => mock.Warning(It.Is<string>(s => s.Contains("Could not parse git stat granularity"))),
                       Times.Once);
        }

        [Fact]
        public void BuildFromConfigDoesNotInvokeCoverageEnrichmentIfReportCouldNotBeConverted()
        {
            var converter = new Mock<ICoverageReportConverter>();
            converter.Setup(mock => mock.Convert(It.IsAny<string>(), It.IsAny<string>()))
                     .Returns(None);
            var enricher = new Mock<IStatsEnricher>();
            enricher.Setup(mock => mock.EnrichWithCoverage(It.IsAny<IRepositorySnapshot>()))
                    .Returns<IRepositorySnapshot>(s => s);
            enricher.Setup(mock => mock.EnrichWithGitStats(It.IsAny<IRepositorySnapshot>(),
                                                           It.IsAny<GitStatGranularity>(),
                                                           It.IsAny<Option<ISubject<int>>>()))
                    .Returns<IRepositorySnapshot, GitStatGranularity, Option<ISubject<int>>>((s, _, __) => s);
            var runner = CreateInitialBuilder()
                         .With(converter.Object)
                         .With(enricher.Object)
                         .Build();

            runner.BuildFromConfig(ConfigFactory(), false);

            enricher.Verify(mock => mock.EnrichWithCoverage(It.IsAny<IRepositorySnapshot>()), Times.Never);
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

            runner.BuildFromConfig(ConfigFactory(), false);

            enricher.Verify(mock => mock.EnrichWithCoverage(It.IsAny<RepositorySnapshot>()), Times.Once);
        }

        [Fact]
        public void BuildFromConfigInvokesGitStatsEnrichmentOnSnapshot()
        {
            var enricher = new Mock<IStatsEnricher>();
            enricher.Setup(mock => mock.EnrichWithCoverage(It.IsAny<IRepositorySnapshot>()))
                    .Returns<IRepositorySnapshot>(s => s);
            enricher.Setup(mock => mock.EnrichWithGitStats(It.IsAny<IRepositorySnapshot>(),
                                                           It.IsAny<GitStatGranularity>(),
                                                           It.IsAny<Option<ISubject<int>>>()))
                    .Returns<IRepositorySnapshot, GitStatGranularity, Option<ISubject<int>>>((s, _, __) => s);
            var runner = CreateInitialBuilder()
                         .With(enricher.Object)
                         .Build();

            runner.BuildFromConfig(ConfigFactory(), false);

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

            runner.BuildFromConfig(ConfigFactory(), false);

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

        [Fact]
        public void RunnerInvokesBuildCommandsIfProvided()
        {
            var config = ConfigFactory();
            var executor = new Mock<ICommandLineExecutor>();
            config.TestCommands = new List<string> { "step one arg", "step two" };
            var runner = CreateInitialBuilder()
                         .With(executor.Object)
                         .Build();

            runner.BuildFromConfig(config, false);

            executor.Verify(mock => mock.Execute("step", "one arg", config.RepoRoot), Times.Once);
            executor.Verify(mock => mock.Execute("step", "two", config.RepoRoot), Times.Once);
        }

        [Fact]
        public void RunnerInvokesTestCommandsIfProvided()
        {
            var config = ConfigFactory();
            var executor = new Mock<ICommandLineExecutor>();
            config.BuildCommands = new List<string> { "step one arg", "step two" };
            var runner = CreateInitialBuilder()
                         .With(executor.Object)
                         .Build();

            runner.BuildFromConfig(config, false);

            executor.Verify(mock => mock.Execute("step", "one arg", config.RepoRoot), Times.Once);
            executor.Verify(mock => mock.Execute("step", "two", config.RepoRoot), Times.Once);
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
            var progressBar = new Mock<IProgressBarFactory>();
            progressBar.Setup(mock => mock.CreateProgressBar(It.IsAny<IObservable<int>>(), It.IsAny<int>()))
                       .Returns(Disposable.Empty);

            return new RunnerBuilder().With(snapshotBuilder.Object)
                                      .With(converter.Object)
                                      .With(enricher.Object)
                                      .With(progressBar.Object);
        }
    }
}
