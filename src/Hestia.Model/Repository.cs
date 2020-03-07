using System;
using System.Linq;
using LanguageExt;

namespace Hestia.Model
{
    public class Repository
    {
        public Repository(long repositoryId,
                          string repositoryName,
                          Option<RepositorySnapshot[]> snapshots,
                          Option<string> coverageExecutionCommand,
                          Option<string> coverageOutputLocation)
        {
            RepositoryId = repositoryId;
            RepositoryName = repositoryName;
            Snapshots = snapshots;
            CoverageExecutionCommand = coverageExecutionCommand;
            CoverageOutputLocation = coverageOutputLocation;
        }

        public long RepositoryId { get; }

        public string RepositoryName { get; }

        public Option<RepositorySnapshot[]> Snapshots { get; }

        public Option<string> CoverageExecutionCommand { get; }

        public Option<string> CoverageOutputLocation { get; }

        public Repository AddSnapshot(RepositorySnapshot snapshot) =>
            new Repository(RepositoryId,
                           RepositoryName,
                           Snapshots.Match(s => s.Concat(new[] { snapshot })
                                                 .ToArray(),
                                           Array.Empty<RepositorySnapshot>),
                           CoverageExecutionCommand,
                           CoverageOutputLocation);

        public RepositoryIdentifier AsRepositoryIdentifier() => new RepositoryIdentifier(RepositoryId, RepositoryName);
    }
}
