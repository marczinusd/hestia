namespace Hestia.Model.Stats
{
    public interface IStatsEnricher
    {
        Repository Enrich(Repository repository,
                          RepositoryStatsEnricherArguments args);

        RepositorySnapshot EnrichWithCoverage(RepositorySnapshot repositorySnapshot);

        RepositorySnapshot EnrichWithGitStats(RepositorySnapshot repositorySnapshot);
    }
}
