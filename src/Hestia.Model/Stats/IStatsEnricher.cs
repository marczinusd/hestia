namespace Hestia.Model.Stats
{
    public interface IStatsEnricher
    {
        RepositorySnapshot EnrichWithCoverage(RepositorySnapshot repositorySnapshot);

        RepositorySnapshot EnrichWithGitStats(RepositorySnapshot repositorySnapshot,
                                              GitStatGranularity granularity);

        Repository Enrich(Repository repository,
                          RepositoryStatsEnricherArguments args);

        File Enrich(File file, string coverageReportPath, string coverageCommand);
    }
}
