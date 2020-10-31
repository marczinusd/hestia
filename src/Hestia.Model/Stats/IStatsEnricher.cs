using System.Reactive.Subjects;
using Hestia.Model.Interfaces;
using LanguageExt;

namespace Hestia.Model.Stats
{
    public interface IStatsEnricher
    {
        IRepositorySnapshot EnrichWithCoverage(IRepositorySnapshot repositorySnapshot);

        IRepositorySnapshot EnrichWithGitStats(IRepositorySnapshot repositorySnapshot,
                                               GitStatGranularity granularity);

        IRepositorySnapshot EnrichWithGitStats(IRepositorySnapshot repositorySnapshot,
                                               GitStatGranularity granularity,
                                               Option<ISubject<int>> progress);

        Repository Enrich(Repository repository,
                          RepositoryStatsEnricherArguments args);
    }
}
