using System.Linq;
using Hestia.Model.Wrappers;

namespace Hestia.Model.Stats
{
    public class StatsEnricher
    {
        private readonly IDiskIOWrapper _ioWrapper;

        public StatsEnricher(IDiskIOWrapper ioWrapper)
        {
            _ioWrapper = ioWrapper;
        }

        // ReSharper disable once UnusedMember.Global
        public Repository Enrich(Repository repository)
        {
            return new Repository(repository.Name, repository.RootDirectory);
        }

        // ReSharper disable once UnusedMember.Global
        public Directory Enrich(Directory directory)
        {
            // ReSharper disable once UnusedVariable
            var result = directory.Files.Select(f => _ioWrapper.ReadAllLinesFromFile(f.Path));

            return new Directory(directory.Name, directory.Path, directory.Directories, directory.Files);
        }

        // ReSharper disable once UnusedMember.Global
        public File Enrich(File file)
        {
            var content = _ioWrapper.ReadAllLinesFromFile(file.Path);

            return new File(content, file.Path, file.Filename, file.Extension);
        }
    }
}
