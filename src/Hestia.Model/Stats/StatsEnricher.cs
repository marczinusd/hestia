using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Hestia.Model.Wrappers;
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
        private readonly IDiskIOWrapper _ioWrapper;

        public StatsEnricher(IDiskIOWrapper ioWrapper,
                             IGitCommands gitCommands)
        {
            _ioWrapper = ioWrapper;
            _gitCommands = gitCommands;
        }

        public Repository Enrich(Repository repository, IEnumerable<Func<Repository, Repository>> enrichers) =>
            enrichers.Fold(repository,
                           (enrichedRepo, enricher) => enricher(enrichedRepo));

        // ReSharper disable once UnusedMember.Global
        public Repository EnrichWithCoverage(Repository repository, string pathToCoverageFile)
        {
            return new Repository(1,
                                  repository.Name,
                                  EnrichWithCoverage(repository.RootDirectory, pathToCoverageFile),
                                  Some(pathToCoverageFile));
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
                                                                                                     f.Path)))
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
            if (coverage.Equals(default))
            {
                return file;
            }

            var enrichedContent =
                _ioWrapper
                    .ReadAllLinesFromFileAsSourceModel(file.Path)
                    .Select(l =>
                    {
                        var coveredLine =
                            coverage.LineCoverages.SingleOrDefault(c => c.lineNumber == l.LineNumber);

                        return coveredLine.Equals(default)
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
            var gitStats = new FileGitStats(_gitCommands.NumberOfChangesForFile(file.Path),
                                            _gitCommands.NumberOfDifferentAuthorsForFile(file.Path));
            var lineStats = _gitCommands.NumberOfDifferentAuthorsAndChangesForLine(file.Path, file.Content.Count)
                                        .Select(x => new LineGitStats(x.lineNumber,
                                                                      x.numberOfCommits,
                                                                      x.numberOfAuthors));

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

        public Repository EnrichWithGitStats(Repository repository)
        {
            return new Repository(repository.Id,
                                  repository.Name,
                                  repository.RootDirectory,
                                  repository.PathToCoverageResultFile);
        }

        private ICoverageProvider ResolveCoverageProvider()
        {
            // TODO
            return new JsonCovCoverageProvider(_ioWrapper);
        }
    }
}
