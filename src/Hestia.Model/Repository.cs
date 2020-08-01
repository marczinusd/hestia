using System.Linq;
using Hestia.Model.Interfaces;
using LanguageExt;

namespace Hestia.Model
{
    public class Repository
    {
        public Repository(int repositoryId,
                          string repositoryName,
                          Option<IRepositorySnapshot[]> snapshots,
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

        public Option<IRepositorySnapshot[]> Snapshots { get; }

        public Option<string> CoverageExecutionCommand { get; }

        public Option<string> CoverageOutputLocation { get; }

        public Repository AddSnapshot(IRepositorySnapshot snapshot) =>
            new Repository(RepositoryId,
                           RepositoryName,
                           Snapshots.Match(s => s.Concat(new[] { snapshot })
                                                 .ToArray(),
                                           new[] { snapshot }),
                           CoverageExecutionCommand,
                           CoverageOutputLocation);

        public Repository With(IRepositorySnapshot[]? snapshots = null,
                               string? coverageExecutionCommand = null,
                               string? coverageOutputLocation = null) =>
            new Repository(RepositoryId,
                           RepositoryName,
                           snapshots ?? Snapshots,
                           coverageExecutionCommand ?? CoverageExecutionCommand,
                           coverageOutputLocation ?? CoverageOutputLocation);
    }
}
