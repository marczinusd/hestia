using System;
using System.IO;
using System.Reactive.Subjects;
using Hestia.DAL.Interfaces;
using Hestia.Model.Builders;
using Hestia.Model.Interfaces;
using Hestia.Model.Stats;
using Hestia.Model.Wrappers;
using LanguageExt;
using Serilog;
using ShellProgressBar;

namespace Hestia.ConsoleRunner
{
    public class Runner
    {
        private readonly IDiskIOWrapper _ioWrapper;
        private readonly IPathValidator _pathValidator;
        private readonly IRepositorySnapshotBuilderWrapper _builder;
        private readonly IStatsEnricher _statsEnricher;
        private readonly ICoverageReportConverter _converter;
        private readonly ILogger _log;
        private readonly ISnapshotPersistence _persistence;

        public Runner(IDiskIOWrapper ioWrapper,
                      IPathValidator pathValidator,
                      IRepositorySnapshotBuilderWrapper builder,
                      IStatsEnricher statsEnricher,
                      ICoverageReportConverter converter,
                      ILogger log,
                      ISnapshotPersistence persistence)
        {
            _ioWrapper = ioWrapper;
            _pathValidator = pathValidator;
            _builder = builder;
            _statsEnricher = statsEnricher;
            _converter = converter;
            _log = log;
            _persistence = persistence;
        }

        public void BuildFromConfig(RunnerConfig config, bool dryRun = false) =>
            ConfigAsBuilder(config)
                .Apply(arguments =>
                {
                    _log.Information("Building snapshot");
                    return _builder.Build(arguments);
                })
                .Apply(snapshot =>
                {
                    _log.Information($"Converting coverage report at {config.CoverageReportLocation} if applicable");
                    return ConvertCoverageReport(snapshot);
                })
                .Apply(snapshot =>
                {
                    _log.Information("Enriching snapshot with coverage stats");
                    return _statsEnricher.EnrichWithCoverage(snapshot);
                })
                .Apply(x =>
                {
                    var canParse =
                        Enum.Parse(typeof(GitStatGranularity), config.StatGranularity, true) is GitStatGranularity;
                    if (!canParse)
                    {
                        _log.Warning($"Could not parse git stat granularity from value {config.StatGranularity}. Possible values: {string.Join(',', Enum.GetValues<GitStatGranularity>())}");
                    }

                    var progressSubject = new Subject<int>();
                    CreateProgressBar(progressSubject, x.Files.Count);
                    var granularity =
                        (GitStatGranularity)Enum.Parse(typeof(GitStatGranularity), config.StatGranularity, true);

                    _log.Information($"Enriching snapshot with git stats with {granularity.ToString()} granularity");
                    return _statsEnricher.EnrichWithGitStats(x, granularity, progressSubject);
                })
                .Apply(snapshot =>
                {
                    if (dryRun)
                    {
                        _log.Information("Dry run, skipping db step");
                        return Unit.Default;
                    }

                    _log.Information("Saving enriched snapshot to database");
                    _persistence.InsertSnapshot(snapshot);
                    _log.Information("Committed to DB successfully");

                    return Unit.Default;
                });

        // ReSharper disable once UnusedMethodReturnValue.Local
        private IDisposable CreateProgressBar(IObservable<int> progressSubject, int total)
        {
            var options = new ProgressBarOptions
            {
                ProgressCharacter = '─',
                ProgressBarOnBottom = true
            };
            var progressBar = new ProgressBar(total, "Processing git stats for all files", options);
            progressSubject.Subscribe(val =>
            {
                progressBar.Tick($"File {val} of {total}");
            });

            return progressBar;
        }

        private RepositorySnapshotBuilderArguments ConfigAsBuilder(RunnerConfig config) =>
            new RepositorySnapshotBuilderArguments(string.Empty,
                                                   config.RepoRoot,
                                                   config.SourceRelativePath,
                                                   config.FileExtensions.ToArray(),
                                                   config.CoverageReportLocation,
                                                   Option<string>.None,
                                                   Option<DateTime>.None,
                                                   _ioWrapper,
                                                   _pathValidator);

        private IRepositorySnapshot ConvertCoverageReport(IRepositorySnapshot snapshot)
        {
            var coverageOutputLocation = snapshot.PathToCoverageResultFile.Match(x => x, string.Empty);
            var newPath = _converter.Convert(coverageOutputLocation,
                                             Path.GetDirectoryName(coverageOutputLocation)!)
                                    .Some(x => x)
                                    .None(() => null!);

            return snapshot.With(pathToCoverageResultFile: newPath);
        }
    }
}
