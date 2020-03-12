using System.Linq;
using LanguageExt;

namespace Hestia.Model
{
    public class Repository
    {
        public Repository(int repositoryId,
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

        public int RepositoryId { get; }

        public string RepositoryName { get; }

        public Option<RepositorySnapshot[]> Snapshots { get; }

        public Option<string> CoverageExecutionCommand { get; }

        public Option<string> CoverageOutputLocation { get; }

        public Repository AddSnapshot(RepositorySnapshot snapshot) =>
            new Repository(RepositoryId,
                           RepositoryName,
                           Snapshots.Match(s => s.Concat(new[] { snapshot })
                                                 .ToArray(),
                                           new[] { snapshot }),
                           CoverageExecutionCommand,
                           CoverageOutputLocation);

        public Repository With(RepositorySnapshot[]? snapshots = null,
                               string? coverageExecutionCommand = null,
                               string? coverageOutputLocation = null) =>
            new Repository(RepositoryId,
                           RepositoryName,
                           snapshots ?? Snapshots,
                           coverageExecutionCommand ?? CoverageExecutionCommand,
                           coverageOutputLocation ?? CoverageOutputLocation);

        public RepositoryIdentifier AsRepositoryIdentifier() => new RepositoryIdentifier(RepositoryId, RepositoryName);
    }
}
