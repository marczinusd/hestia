using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Hestia.Model.Builders;
using Hestia.Model.Interfaces;
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
        private readonly ICoverageReportConverter _converter;
        private readonly ICommandLineExecutor _executor;
        private readonly IGitCommands _gitCommands;
        private readonly IDiskIOWrapper _ioWrapper;
        private readonly ILogger _logger;
        private readonly IPathValidator _pathValidator;
        private readonly ICoverageProviderFactory _providerFactory;

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

        public IRepositorySnapshot EnrichWithCoverage(IRepositorySnapshot repositorySnapshot)
        {
            _logger.Debug($"Enriching repository snapshot at {repositorySnapshot.AtHash} with coverage stats");
            var pathToCoverageFile = match(repositorySnapshot.PathToCoverageResultFile,
                                           s => s,
                                           () =>
                                               throw new
                                                   OptionIsNoneException($"{nameof(repositorySnapshot.PathToCoverageResultFile)} cannot be None"));
            IEnumerable<IFileCoverage> coverages = _providerFactory.CreateProviderForFile(pathToCoverageFile)
                                                                   .ParseFileCoveragesFromFilePath(pathToCoverageFile);

            return repositorySnapshot.With(repositorySnapshot.Files
                                                             .Apply(f => EnrichWithCoverage(f, coverages))
                                                             .ToArray(),
                                           pathToCoverageResultFile: pathToCoverageFile);
        }

        public IRepositorySnapshot EnrichWithGitStats(IRepositorySnapshot repositorySnapshot,
                                                      GitStatGranularity granularity)
        {
            _logger.Debug($"Enriching repository snapshot with hash {repositorySnapshot.AtHash} with git stats");

            return repositorySnapshot.With(repositorySnapshot
                                           .Files
                                           .Apply(x => x.Select(f => EnrichWithGitStats(f, granularity)))
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

        private IRepositorySnapshot ConvertCoverageResults(IRepositorySnapshot repositorySnapshot)
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

        private IEnumerable<IFile> EnrichWithCoverage(IEnumerable<IFile> files, IEnumerable<IFileCoverage> coverages) =>
            files.Select(f => EnrichWithCoverage(f,
                                                 coverages.SingleOrDefault(cov => cov.FileName.Contains(f.Filename))!));

        private IFile EnrichWithCoverage(IFile file, IFileCoverage coverage)
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (coverage == null || coverage.Equals(default))
            {
                return file;
            }

            IEnumerable<ISourceLine> enrichedContent =
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

        private IFile EnrichWithGitStats(IFile file, GitStatGranularity granularity)
        {
            _logger.Debug($"Loading git stats for {file.Filename}");
            var gitStats = new FileGitStats(_gitCommands.NumberOfChangesForFile(file.FullPath),
                                            _gitCommands.NumberOfDifferentAuthorsForFile(file.FullPath));

            var lineStats = new List<ILineGitStats>();
            if (granularity == GitStatGranularity.Full)
            {
                lineStats = _gitCommands.NumberOfDifferentAuthorsAndChangesForLine(file.FullPath, file.Content.Count)
                                        .Select(x => new LineGitStats(x.lineNumber,
                                                                      x.numberOfCommits,
                                                                      x.numberOfAuthors) as ILineGitStats)
                                        .ToList(); // force evaluation
            }

            IEnumerable<ISourceLine> enrichedContent =
                file.Content.Select(line =>
                {
                    // ReSharper disable once AssignNullToNotNullAttribute
                    ILineGitStats statsForLine = lineStats.SingleOrDefault(l => l.LineNumber ==
                                                                               line.LineNumber);
                    return new SourceLine(line.LineNumber,
                                          line.Text,
                                          line.LineCoverageStats,
                                          statsForLine != null ? Some(statsForLine) : Option<ILineGitStats>.None);
                });

            // lineStats.Single(stat => stat.LineNumber ==
            //                               line.LineNumber)))
            return file.With(enrichedContent.ToList(), gitStats);
        }
    }
}
