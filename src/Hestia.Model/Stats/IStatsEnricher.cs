using Hestia.Model.Interfaces;

namespace Hestia.Model.Stats
{
    public interface IStatsEnricher
    {
        IRepositorySnapshot EnrichWithCoverage(IRepositorySnapshot repositorySnapshot);

        IRepositorySnapshot EnrichWithGitStats(IRepositorySnapshot repositorySnapshot,
                                               GitStatGranularity granularity);

        Repository Enrich(Repository repository,
                          RepositoryStatsEnricherArguments args);

        IFile Enrich(IFile file, string coverageReportPath, string coverageCommand);
    }
}
