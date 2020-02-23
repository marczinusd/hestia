using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Hestia.Model.Wrappers;
using LanguageExt;
using static LanguageExt.Prelude;

namespace Hestia.Model.Stats
{
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

        [Pure]
        public Repository Enrich(Repository repository, IEnumerable<Func<Repository, Repository>> enrichers) =>
            enrichers.Fold(repository,
                           (enrichedRepo, enricher) => enricher(enrichedRepo));

        // ReSharper disable once UnusedMember.Global
        [Pure]
        public Repository EnrichWithCoverage(Repository repository, string pathToCoverageFile)
        {
            return new Repository(1,
                                  repository.Name,
                                  repository.RootDirectory,
                                  Some(pathToCoverageFile));
        }

        // ReSharper disable once UnusedMember.Global
        [Pure]
        public Directory EnrichWithCoverage(Directory directory)
        {
            // ReSharper disable once UnusedVariable
            var result = directory.Files.Select(f => _ioWrapper.ReadAllLinesFromFileAsSourceModel(f.Path));

            return new Directory(directory.Name,
                                 directory.Path,
                                 directory.Directories,
                                 directory.Files);
        }

        // ReSharper disable once UnusedMember.Global
        [Pure]
        public File EnrichWithCoverage(File file)
        {
            var content = _ioWrapper.ReadAllLinesFromFileAsSourceModel(file.Path);

            return new File(0,
                            file.Filename,
                            file.Extension,
                            file.Path,
                            content,
                            Option<FileGitStats>.None,
                            Option<FileCoverageStats>.None);
        }

        public Repository EnrichWithGitStats(Repository repository)
        {
            return new Repository(repository.Id,
                                  repository.Name,
                                  repository.RootDirectory,
                                  repository.PathToCoverageResultFile);
        }

        // ReSharper disable once UnusedMember.Local
        [Pure]
        private FileGitStats CollectGitStats(string fromFilePath)
        {
            return new FileGitStats(_gitCommands.NumberOfChangesForFile(fromFilePath), _gitCommands.NumberOfDifferentAuthorsForFile(fromFilePath));
        }

        // ReSharper disable once UnusedMember.Local
        // ReSharper disable once UnusedParameter.Local
        private Repository CollectCoverageStats(Repository repository)
        {
            return repository;
        }
    }
}
