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
        private IDiskIOWrapper _diskIOWrapper = Mock.Of<IDiskIOWrapper>();
        private IPathValidator _pathValidator = Mock.Of<IPathValidator>();

        private IRepositorySnapshotBuilderWrapper
            _snapshotBuilderWrapper = Mock.Of<IRepositorySnapshotBuilderWrapper>();

        private IStatsEnricher _enricher = Mock.Of<IStatsEnricher>();
        private ICoverageReportConverter _converter = Mock.Of<ICoverageReportConverter>();
        private ILogger _logger = Mock.Of<ILogger>();
        private ISnapshotPersistence _snapshotPersistence = Mock.Of<ISnapshotPersistence>();
        private IProgressBarFactory _progressBarFactory = Mock.Of<IProgressBarFactory>();

        public RunnerBuilder With(IDiskIOWrapper diskIOWrapper)
        {
            _diskIOWrapper = diskIOWrapper;
            return this;
        }

        public RunnerBuilder With(IPathValidator pathValidator)
        {
            _pathValidator = pathValidator;
            return this;
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

        public Runner Build() => new Runner(_diskIOWrapper,
                                            _pathValidator,
                                            _snapshotBuilderWrapper,
                                            _enricher,
                                            _converter,
                                            _logger,
                                            _snapshotPersistence,
                                            _progressBarFactory);
    }
}
