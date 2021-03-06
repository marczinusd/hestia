﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Subjects;
using System.Text.RegularExpressions;
using Hestia.DAL.Interfaces;
using Hestia.Model.Builders;
using Hestia.Model.Interfaces;
using Hestia.Model.Stats;
using Hestia.Model.Wrappers;
using LanguageExt;
using Serilog;

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
        private readonly IProgressBarFactory _progressBarFactory;
        private readonly ICommandLineExecutor _executor;
        private readonly ISpinner _spinner;

        public Runner(IDiskIOWrapper ioWrapper,
                      IPathValidator pathValidator,
                      IRepositorySnapshotBuilderWrapper builder,
                      IStatsEnricher statsEnricher,
                      ICoverageReportConverter converter,
                      ILogger log,
                      ISnapshotPersistence persistence,
                      IProgressBarFactory progressBarFactory,
                      ICommandLineExecutor executor,
                      ISpinner spinner)
        {
            _ioWrapper = ioWrapper;
            _pathValidator = pathValidator;
            _builder = builder;
            _statsEnricher = statsEnricher;
            _converter = converter;
            _log = log;
            _persistence = persistence;
            _progressBarFactory = progressBarFactory;
            _executor = executor;
            _spinner = spinner;
        }

        public Unit BuildFromConfig(RunnerConfig config, bool dryRun)
        {
            Unit CommitResultsToDb(IRepositorySnapshot snapshot)
            {
                if (dryRun)
                {
                    _log.Information("Dry run, skipping db step");
                    return Unit.Default;
                }

                _log.Information("Saving enriched snapshot to database");
                var result = _persistence.InsertSnapshotSync(snapshot);
                _log.Information($"Committed {result} state entries to DB successfully");

                return Unit.Default;
            }

            IRepositorySnapshot EnrichSnapshotWithGitStats(IRepositorySnapshot snapshot)
            {
                var canParse = Enum.GetNames<GitStatGranularity>()
                                   .Select(val => val.ToLower())
                                   .Contains(config.StatGranularity);
                if (!canParse)
                {
                    _log.Warning($"Could not parse git stat granularity from value {config.StatGranularity}. Possible values: {string.Join(',', Enum.GetValues<GitStatGranularity>())}");
                }

                var progressSubject = new Subject<int>();
                var disposable = _progressBarFactory.CreateProgressBar(progressSubject, snapshot.Files.Count);
                var granularity = !canParse
                                      ? GitStatGranularity.File
                                      : (GitStatGranularity)Enum.Parse(typeof(GitStatGranularity),
                                                                       config.StatGranularity,
                                                                       true);

                _log.Information($"Enriching snapshot with git stats with {granularity.ToString()} granularity");
                var result = _statsEnricher.EnrichWithGitStats(snapshot, granularity, progressSubject);
                disposable.Dispose();

                return result;
            }

            IRepositorySnapshot EnrichSnapshotWithCoverage(IRepositorySnapshot snapshot)
            {
                _log.Information("Enriching snapshot with coverage stats");
                var coveragePath = snapshot.PathToCoverageResultFile
                                           .Match(x => x, () => string.Empty);
                if (string.IsNullOrWhiteSpace(coveragePath))
                {
                    _log.Warning($"Coverage report provided at {coveragePath} could not be converted and was not processed");
                    return snapshot;
                }

                return _statsEnricher.EnrichWithCoverage(snapshot);
            }

            IRepositorySnapshot FilterFilesBasedOnIgnorePatterns(IRepositorySnapshot snapshot)
            {
                return snapshot.With(files: snapshot.Files.Where(f => FilterFileOnPatterns(config.IgnorePatterns,
                                                                     f)));
            }

            IRepositorySnapshot ConvertCoverageReportIfApplicable(IRepositorySnapshot snapshot)
            {
                _log.Information($"Converting coverage report at {config.CoverageReportLocation} if applicable");
                return ConvertCoverageReport(snapshot);
            }

            IRepositorySnapshot BuildSnapshot(RepositorySnapshotBuilderArguments arguments)
            {
                _log.Information("Building snapshot");
                return _builder.Build(arguments);
            }

            RepositorySnapshotBuilderArguments RunTestsIfApplicable(RepositorySnapshotBuilderArguments arguments)
            {
                if (config.TestCommands == null || !config.TestCommands.Any())
                {
                    _log.Information("No test commands were provided -- skipping");

                    return arguments;
                }

                _log.Information($"Triggering test command '{string.Join(" && ", config.TestCommands)}' in directory: {config.RepoRoot}");
                _spinner.Start("Running tests...",
                               () => config.TestCommands.Iter(command =>
                               {
                                   var split = command.Split(' ');
                                   _executor.Execute(split.Head(), string.Join(' ', split.Tail()), config.RepoRoot);
                               }));

                return arguments;
            }

            RepositorySnapshotBuilderArguments RunBuildIfApplicable(
                RepositorySnapshotBuilderArguments repositorySnapshotBuilderArguments)
            {
                if (config.BuildCommands == null || !config.BuildCommands.Any())
                {
                    _log.Information("No build commands were provided -- skipping");

                    return repositorySnapshotBuilderArguments;
                }

                _log.Information($"Triggering build command '{string.Join(" && ", config.BuildCommands)}' in directory: {config.RepoRoot}");
                _spinner.Start("Build in progress...",
                               () => config.BuildCommands.Iter(command =>
                               {
                                   var split = command.Split(' ');
                                   _executor.Execute(split.Head(), string.Join(' ', split.Tail()), config.RepoRoot);
                               }));

                return repositorySnapshotBuilderArguments;
            }

            return ConfigAsBuilder(config)
                   .Apply(RunBuildIfApplicable)
                   .Apply(RunTestsIfApplicable)
                   .Apply(BuildSnapshot)
                   .Apply(ConvertCoverageReportIfApplicable)
                   .Apply(FilterFilesBasedOnIgnorePatterns)
                   .Apply(EnrichSnapshotWithCoverage)
                   .Apply(EnrichSnapshotWithGitStats)
                   .Apply(CommitResultsToDb);
        }

        private bool FilterFileOnPatterns(List<string> ignorePatterns, IFile file)
        {
            foreach (var pattern in ignorePatterns)
            {
                try
                {
                    if (Regex.IsMatch(file.FullPath, pattern))
                    {
                        _log.Debug($"Filtering out {file.Path} because it matched pattern: '{pattern}'");
                        return false;
                    }
                }
                catch (ArgumentException)
                {
                    _log.Warning($"Regex pattern {pattern} is invalid -- skipping");
                }
            }

            return true;
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
                                             Path.GetDirectoryName(coverageOutputLocation)!);

            return snapshot.With(pathToCoverageResultFile: newPath);
        }
    }
}
