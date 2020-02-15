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
        public Repository Enrich(Repository repository)
        {
            return new Repository(repository.Name,
                                  repository.RootDirectory,
                                  Option<string>.None,
                                  1);
        }

        // ReSharper disable once UnusedMember.Global
        public Directory Enrich(Directory directory)
        {
            // ReSharper disable once UnusedVariable
            var result = directory.Files.Select(f => _ioWrapper.ReadAllLinesFromFile(f.Path));

            return new Directory(directory.Name,
                                 directory.Path,
                                 directory.Directories,
                                 directory.Files);
        }

        // ReSharper disable once UnusedMember.Global
        public File Enrich(File file)
        {
            var content = _ioWrapper.ReadAllLinesFromFile(file.Path);

            return new File(content,
                            file.Path,
                            file.Filename,
                            file.Extension,
                            Option<FileGitStats>.None,
                            Option<FileCoverageStats>.None);
        }

        // ReSharper disable once UnusedMember.Local
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
