using System;
using Hestia.ConsoleRunner;
using Hestia.DAL.Interfaces;
using Hestia.Model.Builders;
using Hestia.Model.Stats;
using Hestia.Model.Wrappers;
using Moq;
using Serilog;

namespace Test.Hestia.ConsoleRunner
{
    public class RunnerBuilder
    {
        private readonly IDiskIOWrapper _diskIOWrapper = Mock.Of<IDiskIOWrapper>();
        private readonly IPathValidator _pathValidator = Mock.Of<IPathValidator>();

        private IRepositorySnapshotBuilderWrapper
            _snapshotBuilderWrapper = Mock.Of<IRepositorySnapshotBuilderWrapper>();

        private IStatsEnricher _enricher = Mock.Of<IStatsEnricher>();
        private ICoverageReportConverter _converter = Mock.Of<ICoverageReportConverter>();
        private ILogger _logger = Mock.Of<ILogger>();
        private ISnapshotPersistence _snapshotPersistence = Mock.Of<ISnapshotPersistence>();
        private IProgressBarFactory _progressBarFactory = Mock.Of<IProgressBarFactory>();
        private ICommandLineExecutor _executor = Mock.Of<ICommandLineExecutor>();
        private ISpinner _spinner;

        public RunnerBuilder()
        {
            var spinnerMock = new Mock<ISpinner>();
            spinnerMock.Setup(mock => mock.Start(It.IsAny<string>(), It.IsAny<Action>()))
                       .Callback<string, Action>((
                                                     _,
                                                     action) =>
                                                 {
                                                     action();
                                                 });

            _spinner = spinnerMock.Object;
        }

        public RunnerBuilder With(IRepositorySnapshotBuilderWrapper snapshotBuilderWrapper)
        {
            _snapshotBuilderWrapper = snapshotBuilderWrapper;
            return this;
        }

        public RunnerBuilder With(ICoverageReportConverter converter)
        {
            _converter = converter;
            return this;
        }

        public RunnerBuilder With(IStatsEnricher enricher)
        {
            _enricher = enricher;
            return this;
        }

        public RunnerBuilder With(ILogger logger)
        {
            _logger = logger;
            return this;
        }

        public RunnerBuilder With(ISnapshotPersistence snapshotPersistence)
        {
            _snapshotPersistence = snapshotPersistence;
            return this;
        }

        public RunnerBuilder With(IProgressBarFactory progressBarFactory)
        {
            _progressBarFactory = progressBarFactory;
            return this;
        }

        public RunnerBuilder With(ICommandLineExecutor executor)
        {
            _executor = executor;
            return this;
        }

        public Runner Build() => new Runner(_diskIOWrapper,
                                            _pathValidator,
                                            _snapshotBuilderWrapper,
                                            _enricher,
                                            _converter,
                                            _logger,
                                            _snapshotPersistence,
                                            _progressBarFactory,
                                            _executor,
                                            _spinner);
    }
}
