using System.Diagnostics.Contracts;
using System.Linq;
using Hestia.Model.Wrappers;
using LanguageExt;

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

        // ReSharper disable once UnusedMember.Global
        [Pure]
        public Repository Enrich(Repository repository)
        {
            return new Repository(1,
                                  repository.Name,
                                  repository.RootDirectory,
                                  Option<string>.None);
        }

        // ReSharper disable once UnusedMember.Global
        [Pure]
        public Directory Enrich(Directory directory)
        {
            // ReSharper disable once UnusedVariable
            var result = directory.Files.Select(f => _ioWrapper.ReadAllLinesFromFile(f.Path));

            return new Directory(0, directory.Name, directory.Path,
                                 directory.Directories,
                                 directory.Files);
        }

        // ReSharper disable once UnusedMember.Global
        [Pure]
        public File Enrich(File file)
        {
            var content = _ioWrapper.ReadAllLinesFromFile(file.Path);

            return new File(0,
                            file.Filename,
                            file.Extension,
                            file.Path,
                            content,
                            Option<FileGitStats>.None,
                            Option<FileCoverageStats>.None);
        }

        // ReSharper disable once UnusedMember.Local
        [Pure]
        private FileGitStats CollectGitStats(string fromFilePath)
        {
            return new FileGitStats(_gitCommands.NumberOfChangesForFile(fromFilePath));
        }

        // ReSharper disable once UnusedMember.Local
        // ReSharper disable once UnusedParameter.Local
        private Repository CollectCoverageStats(Repository repository)
        {
            return repository;
        }
    }
}
