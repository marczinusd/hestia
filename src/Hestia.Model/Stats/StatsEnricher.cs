using System.Linq;
using Hestia.Model.Builders;
using Hestia.Model.Wrappers;
using LanguageExt;
using Microsoft.Extensions.Logging;
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
        private readonly IGitCommands _gitCommands;
        private readonly IDiskIOWrapper _ioWrapper;
        private readonly ILogger<IStatsEnricher> _logger;

        public StatsEnricher(IDiskIOWrapper ioWrapper,
                             IGitCommands gitCommands,
                             ILogger<IStatsEnricher> logger,
                             ICommandLineExecutor executor)
        {
            _ioWrapper = ioWrapper;
            _gitCommands = gitCommands;
            _logger = logger;
            _executor = executor;
        }

        public Repository Enrich(Repository repository,
                                 RepositoryStatsEnricherArguments args)
        {
            var initialSnapshotId = 1;
            var repoArgs = new RepositorySnapshotBuilderArguments(initialSnapshotId,
                                                                  args.RepoPath,
                                                                  args.SourceRoot,
                                                                  args.SourceExtensions,
                                                                  args.CoverageOutputLocation,
                                                                  Option<string>.None,
                                                                  _ioWrapper,
                                                                  new PathValidator());
            var sampleInterval = (args.LastCommitToSample - args.FirstCommitToSample) / args.NumberOfSamples;

            return Enumerable.Range(0, args.NumberOfSamples)
                             .Select(i => args.FirstCommitToSample + (i * sampleInterval))
                             .Select(commitNo => _gitCommands.GetHashForNthCommit(args.RepoPath, commitNo))
                             .ToList() // force eval
                             .Fold(repository,
                                   (repo, hash) =>
                                   {
                                       _gitCommands.Checkout(hash, args.RepoPath);
                                       _executor.Execute(args.CoverageCommand, string.Empty, args.RepoPath);

                                       return repoArgs.With(initialSnapshotId + 1, hash)
                                                      .Build()
                                                      .Apply(EnrichWithCoverage)
                                                      .Apply(EnrichWithGitStats)
                                                      .Apply(repo.AddSnapshot);
                                   });
        }

        // ReSharper disable once UnusedMember.Global
        public RepositorySnapshot EnrichWithCoverage(RepositorySnapshot repositorySnapshot)
        {
            _logger.LogDebug($"Enriching repository snapshot at {repositorySnapshot.AtHash} with coverage stats");
            var pathToCoverageFile = match(repositorySnapshot.PathToCoverageResultFile,
                                           s => s,
                                           () =>
                                               throw new
                                                   OptionIsNoneException($"{nameof(repositorySnapshot.PathToCoverageResultFile)} cannot be None"));

            return repositorySnapshot.With(repositorySnapshot.RootDirectory.Apply(dir => EnrichWithCoverage(dir,
                                                                                                            pathToCoverageFile)),
                                           pathToCoverageResultFile: pathToCoverageFile);
        }

        public RepositorySnapshot EnrichWithGitStats(RepositorySnapshot repositorySnapshot)
        {
            _logger.LogDebug($"Enriching repository snapshot with hash {repositorySnapshot.AtHash} with git stats");

            return repositorySnapshot.With(repositorySnapshot.RootDirectory.Apply(EnrichWithGitStats));
        }

        // ReSharper disable once UnusedMember.Global
        private Directory EnrichWithCoverage(Directory directory, string coveragePath) =>
            directory.With(directory.Files
                                    .Select(f =>
                                    {
                                        var coverage = ResolveCoverageProvider()
                                                       .ParseFileCoveragesFromFilePath(coveragePath)
                                                       .SingleOrDefault(c => c.FileName == f.FullPath);

                                        return EnrichWithCoverage(f, coverage);
                                    })
                                    .ToList(),
                           directory.Directories
                                    .Select(d => EnrichWithCoverage(d, coveragePath))
                                    .ToList());

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

        private File EnrichWithGitStats(File file)
        {
            _logger.LogInformation($"Loading git stats for {file.Filename}");
            var gitStats = new FileGitStats(_gitCommands.NumberOfChangesForFile(file.FullPath),
                                            _gitCommands.NumberOfDifferentAuthorsForFile(file.FullPath));
            var lineStats = _gitCommands.NumberOfDifferentAuthorsAndChangesForLine(file.FullPath, file.Content.Count)
                                        .Select(x => new LineGitStats(x.lineNumber,
                                                                      x.numberOfCommits,
                                                                      x.numberOfAuthors))
                                        .ToList(); // force evaluation

            var enrichedContent =
                file.Content.Select(line => new SourceLine(line.LineNumber,
                                                           line.Text,
                                                           line.LineCoverageStats,
                                                           Some(lineStats.Single(stat => stat.LineNumber ==
                                                                                         line.LineNumber))));

            return file.With(enrichedContent.ToList(), gitStats);
        }

        private Directory EnrichWithGitStats(Directory directory) =>
            directory.With(directory.Files
                                    .Select(EnrichWithGitStats)
                                    .ToList(),
                           directory.Directories
                                    .Select(EnrichWithGitStats)
                                    .ToList());

        // TODO: actually implement this once multiple providers are added
        private ICoverageProvider ResolveCoverageProvider() => new JsonCovCoverageProvider(_ioWrapper);
    }
}
