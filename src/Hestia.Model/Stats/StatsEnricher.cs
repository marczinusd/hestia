using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Hestia.Model.Builders;
using Hestia.Model.Wrappers;
using LanguageExt;
using Microsoft.Extensions.Logging;
using static LanguageExt.Prelude;

namespace Hestia.Model.Stats
{
    /// <summary>
    /// Provides easy to use methods to enrich raw repos/directories/files with git/coverage stats.
    /// </summary>
    /// <remarks>
    /// Do note that all of these operations are **very** costly -- even more so if you're using Windows.
    /// </remarks>
    public class StatsEnricher
    {
        private readonly IGitCommands _gitCommands;
        private readonly ILogger<StatsEnricher> _logger;
        private readonly ICommandLineExecutor _executor;
        private readonly IDiskIOWrapper _ioWrapper;

        public StatsEnricher(IDiskIOWrapper ioWrapper,
                             IGitCommands gitCommands,
                             ILogger<StatsEnricher> logger,
                             ICommandLineExecutor executor)
        {
            _ioWrapper = ioWrapper;
            _gitCommands = gitCommands;
            _logger = logger;
            _executor = executor;
        }

        public Repository Enrich(Repository repository,
                                 string repoPath,
                                 string sourceRoot,
                                 string[] sourceExtensions,
                                 string coverageCommand,
                                 string coverageOutputLocation,
                                 int firstCommitToSample,
                                 int lastCommitToSample,
                                 int numberOfSamples)
        {
            var args = new RepositorySnapshotBuilderArguments(-1,
                                                              repoPath,
                                                              sourceRoot,
                                                              sourceExtensions,
                                                              coverageOutputLocation,
                                                              Option<string>.None,
                                                              _ioWrapper,
                                                              new PathValidator());
            var sampleInterval = (lastCommitToSample - firstCommitToSample) / numberOfSamples;

            return Enumerable.Range(0, numberOfSamples)
                             .Select(i => firstCommitToSample + (i * sampleInterval))
                             .Select(commitNo => _gitCommands.GetHashForNthCommit(repoPath, commitNo))
                             .ToList() // force eval
                             .Fold(repository,
                                   (repo, hash) =>
                                   {
                                       _gitCommands.Checkout(hash, repoPath);
                                       _executor.Execute(coverageCommand, string.Empty, repoPath);

                                       return args.With(hash)
                                                  .Build()
                                                  .Apply(EnrichWithCoverage)
                                                  .Apply(EnrichWithGitStats)
                                                  .Apply(repo.AddSnapshot);
                                   });
        }

        public RepositorySnapshot Enrich(RepositorySnapshot repositorySnapshot,
                                         IEnumerable<Func<RepositorySnapshot, RepositorySnapshot>> enrichers) =>
            enrichers.Fold(repositorySnapshot,
                           (enrichedRepo, enricher) => enricher(enrichedRepo));

        // ReSharper disable once UnusedMember.Global
        public RepositorySnapshot EnrichWithCoverage(RepositorySnapshot repositorySnapshot)
        {
            _logger.LogDebug($"Enriching repository snapshot at {repositorySnapshot.AtHash} with git stats");
            var pathToCoverageFile = match(repositorySnapshot.PathToCoverageResultFile,
                                           s => s,
                                           () =>
                                               throw new
                                                   OptionIsNoneException($"{nameof(repositorySnapshot.PathToCoverageResultFile)} cannot be None"));

            return new RepositorySnapshot(1,
                                          EnrichWithCoverage(repositorySnapshot.RootDirectory, pathToCoverageFile),
                                          Some(pathToCoverageFile),
                                          Option<string>.None);
        }

        // ReSharper disable once UnusedMember.Global
        public Directory EnrichWithCoverage(Directory directory, string coveragePath) =>
            new Directory(directory.Name,
                          directory.Path,
                          directory.Directories.Select(d => EnrichWithCoverage(d, coveragePath))
                                   .ToList(),
                          directory.Files.Select(f => EnrichWithCoverage(f,
                                                                         ResolveCoverageProvider()
                                                                             .ParseFileCoveragesFromFilePath(coveragePath)
                                                                             .SingleOrDefault(cov => cov.FileName ==
                                                                                                     Path.Join(f.Path,
                                                                                                               f.Filename))))
                                   .ToList());

        public IObservable<File> EnrichObservable(File file, IEnumerable<FileCoverage> fileCoverages) =>
            EnrichWithGitStatsObservable(file)
                .Merge(EnrichWithCoverageObservable(file, fileCoverages));

        public IObservable<File> EnrichWithCoverageObservable(File file, IEnumerable<FileCoverage> fileCoverages) =>
            Observable.Create<File>(s =>
            {
                s.OnNext(EnrichWithCoverage(file, fileCoverages.Single(f => f.FileName == file.Path)));
                s.OnCompleted();

                return Disposable.Empty;
            });

        public File EnrichWithCoverage(File file, FileCoverage coverage)
        {
            _logger.LogInformation($"Loading coverage information for {file.Filename}");
            if (coverage == null || coverage.Equals(default))
            {
                return file;
            }

            var enrichedContent =
                _ioWrapper
                    .ReadAllLinesFromFileAsSourceModel(Path.Join(file.Path, file.Filename))
                    .Select(l =>
                    {
                        var coveredLine =
                            coverage.LineCoverages.SingleOrDefault(c => c.LineNumber == l.LineNumber);

                        return coveredLine == null || coveredLine.Equals(default)
                                   ? l
                                   : new SourceLine(l.LineNumber,
                                                    l.Text,
                                                    Some(new LineCoverageStats()),
                                                    l.LineGitStats);
                    });

            return new File(0,
                            file.Filename,
                            file.Extension,
                            file.Path,
                            enrichedContent.ToList(),
                            file.GitStats,
                            Some(new FileCoverageStats(coverage)));
        }

        public IObservable<File> EnrichWithGitStatsObservable(File file) =>
            Observable.Create<File>(s =>
            {
                s.OnNext(EnrichWithGitStats(file));
                s.OnCompleted();

                return Disposable.Empty;
            });

        public File EnrichWithGitStats(File file)
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

            return new File(file.Id,
                            file.Filename,
                            file.Extension,
                            file.Path,
                            enrichedContent.ToList(),
                            gitStats,
                            file.CoverageStats);
        }

        public RepositorySnapshot EnrichWithGitStats(RepositorySnapshot repositorySnapshot) =>
            new RepositorySnapshot(repositorySnapshot.SnapshotId,
                                   EnrichWithGitStats(repositorySnapshot.RootDirectory),
                                   repositorySnapshot.PathToCoverageResultFile,
                                   Option<string>.None);

        public Directory EnrichWithGitStats(Directory directory) =>
            new Directory(directory.Name,
                          directory.Path,
                          directory.Directories.Select(EnrichWithGitStats)
                                   .ToList(),
                          directory.Files.Select(EnrichWithGitStats)
                                   .ToList());

        // TODO: actually implement this once multiple providers are added
        private ICoverageProvider ResolveCoverageProvider() => new JsonCovCoverageProvider(_ioWrapper);
    }
}
