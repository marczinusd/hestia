﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Hestia.Model.Builders;
using Hestia.Model.Wrappers;
using LanguageExt;
using Serilog;
using static LanguageExt.Prelude;

namespace Hestia.Model.Stats
{
    /// <summary>
    ///     Provides easy to use methods to enrich raw repos/directories/files with git/coverage stats.
    /// </summary>
    /// <remarks>
    ///     Do note that all of these operations are **very** costly -- even more so if you're using Windows.
    /// </remarks>
    public class StatsEnricher : IStatsEnricher
    {
        private readonly ICommandLineExecutor _executor;
        private readonly ICoverageProviderFactory _providerFactory;
        private readonly IPathValidator _pathValidator;
        private readonly ICoverageReportConverter _converter;
        private readonly IGitCommands _gitCommands;
        private readonly IDiskIOWrapper _ioWrapper;
        private readonly ILogger _logger;

        public StatsEnricher(IDiskIOWrapper ioWrapper,
                             IGitCommands gitCommands,
                             ILogger logger,
                             ICommandLineExecutor executor,
                             ICoverageProviderFactory providerFactory,
                             IPathValidator pathValidator,
                             ICoverageReportConverter converter)
        {
            _ioWrapper = ioWrapper;
            _gitCommands = gitCommands;
            _logger = logger;
            _executor = executor;
            _providerFactory = providerFactory;
            _pathValidator = pathValidator;
            _converter = converter;
        }

        public RepositorySnapshot EnrichWithCoverage(RepositorySnapshot repositorySnapshot)
        {
            _logger.Debug($"Enriching repository snapshot at {repositorySnapshot.AtHash} with coverage stats");
            var pathToCoverageFile = match(repositorySnapshot.PathToCoverageResultFile,
                                           s => s,
                                           () =>
                                               throw new
                                                   OptionIsNoneException($"{nameof(repositorySnapshot.PathToCoverageResultFile)} cannot be None"));
            var coverages = _providerFactory.CreateProviderForFile(pathToCoverageFile)
                                            .ParseFileCoveragesFromFilePath(pathToCoverageFile);

            return repositorySnapshot.With(repositorySnapshot.Files
                                                             .Apply(f => EnrichWithCoverage(f, coverages))
                                                             .ToArray(),
                                           pathToCoverageResultFile: pathToCoverageFile);
        }

        public RepositorySnapshot EnrichWithGitStats(RepositorySnapshot repositorySnapshot,
                                                     GitStatGranularity granularity)
        {
            _logger.Debug($"Enriching repository snapshot with hash {repositorySnapshot.AtHash} with git stats");

            return repositorySnapshot.With(repositorySnapshot
                                           .Files.Apply(x => x.Select(f => EnrichWithGitStats(f, granularity)))
                                           .ToList());
        }

        public Repository Enrich(Repository repository,
                                 RepositoryStatsEnricherArguments args)
        {
            var initialSnapshotId = string.Empty;
            var repoArgs = new RepositorySnapshotBuilderArguments(initialSnapshotId,
                                                                  args.RepoPath,
                                                                  args.SourceRoot,
                                                                  args.SourceExtensions,
                                                                  args.CoverageOutputLocation,
                                                                  Option<string>.None,
                                                                  Option<DateTime>.None,
                                                                  _ioWrapper,
                                                                  _pathValidator);
            var sampleInterval = (args.LastCommitToSample - args.FirstCommitToSample) / (args.NumberOfSamples - 1);

            return Enumerable
                   .Range(0, args.NumberOfSamples - 1) // -1 so last sample should come from args.LastCommitToSample
                   .Select(i => args.FirstCommitToSample + (i * sampleInterval))
                   .Append(args.LastCommitToSample)
                   .Select(commitNo => _gitCommands.GetHashForNthCommit(args.RepoPath, commitNo))
                   .ToList() // force eval
                   .Fold(repository,
                         (repo, hash) =>
                         {
                             _gitCommands.Checkout(hash, args.RepoPath);
                             _executor.Execute(args.CoverageCommand, string.Empty, args.RepoPath);
                             var commitCreationDate = _gitCommands.DateOfLatestCommitOnBranch(args.RepoPath);

                             return repoArgs.With(initialSnapshotId, hash, commitCreationDate)
                                            .Build()
                                            .Apply(ConvertCoverageResults)
                                            .Apply(EnrichWithCoverage)
                                            .Apply(x => EnrichWithGitStats(x, GitStatGranularity.File))
                                            .Apply(repo.AddSnapshot);
                         });
        }

        public File Enrich(File file, string coverageReportPath, string coverageCommand)
        {
            if (string.IsNullOrWhiteSpace(coverageReportPath) || string.IsNullOrWhiteSpace(coverageCommand))
            {
                throw new
                    ArgumentOutOfRangeException($"Either {nameof(coverageReportPath)} or {nameof(coverageReportPath)} have to be non-null and non-empty.");
            }

            _executor.Execute(coverageCommand, string.Empty, string.Empty);

            var finalPath = coverageReportPath;
            if (!coverageReportPath.Contains("coverage.json"))
            {
                finalPath = _converter.Convert(coverageReportPath,
                                               Path.GetDirectoryName(coverageReportPath) ?? coverageReportPath)
                                      .Some(x => x)
                                      .None(() => coverageReportPath);
            }

            var coverage = _providerFactory.CreateProviderForFile(finalPath)
                                           .ParseFileCoveragesFromFilePath(finalPath)
                                           .Single(f => f.FileName.Equals(file.Filename));

            return file.With(coverageStats: new FileCoverageStats(coverage));
        }

        private RepositorySnapshot ConvertCoverageResults(RepositorySnapshot repositorySnapshot)
        {
            var path = repositorySnapshot.PathToCoverageResultFile.Some(x => x)
                                         .None(() => string.Empty);
            if (path.ToLower()
                    .Contains("coverage.json"))
            {
                return repositorySnapshot;
            }

            var result = _converter.Convert(path,
                                            Path.GetDirectoryName(path) ??
                                            throw new DirectoryNotFoundException($"Invalid path provided: {path}"));
            if (result.IsSome)
            {
                return repositorySnapshot.With(pathToCoverageResultFile: result.Some(x => x)
                                                                               .None(string.Empty));
            }

            return repositorySnapshot;
        }

        private IEnumerable<File> EnrichWithCoverage(IEnumerable<File> files, IEnumerable<FileCoverage> coverages) =>
            files.Select(f => EnrichWithCoverage(f,
                                                 coverages.SingleOrDefault(cov => cov.FileName.Contains(f.Filename))));

        private File EnrichWithCoverage(File file, FileCoverage coverage)
        {
            if (coverage == null || coverage.Equals(default))
            {
                return file;
            }

            var enrichedContent =
                _ioWrapper
                    .ReadAllLinesFromFileAsSourceModel(file.FullPath)
                    .Select(l =>
                    {
                        var coveredLine =
                            coverage.LineCoverages.SingleOrDefault(c => c.LineNumber == l.LineNumber);

                        return coveredLine == null || coveredLine.Equals(default)
                                   ? l
                                   : l.With(new LineCoverageStats(isCovered: true));
                    });

            return file.With(enrichedContent.ToList(),
                             coverageStats: new FileCoverageStats(coverage));
        }

        private File EnrichWithGitStats(File file, GitStatGranularity granularity)
        {
            _logger.Debug($"Loading git stats for {file.Filename}");
            var gitStats = new FileGitStats(_gitCommands.NumberOfChangesForFile(file.FullPath),
                                            _gitCommands.NumberOfDifferentAuthorsForFile(file.FullPath));

            var lineStats = new List<LineGitStats>();
            if (granularity == GitStatGranularity.Full)
            {
                lineStats = _gitCommands.NumberOfDifferentAuthorsAndChangesForLine(file.FullPath, file.Content.Count)
                                        .Select(x => new LineGitStats(x.lineNumber,
                                                                      x.numberOfCommits,
                                                                      x.numberOfAuthors))
                                        .ToList(); // force evaluation
            }

            var enrichedContent =
                file.Content.Select(line => new SourceLine(line.LineNumber,
                                                           line.Text,
                                                           line.LineCoverageStats,
                                                           lineStats.SingleOrDefault(l => l.LineNumber ==
                                                                                          line.LineNumber)));

            // lineStats.Single(stat => stat.LineNumber ==
            //                               line.LineNumber)))
            return file.With(enrichedContent.ToList(), gitStats);
        }
    }
}
