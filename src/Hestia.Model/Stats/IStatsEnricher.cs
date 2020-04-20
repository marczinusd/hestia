namespace Hestia.Model.Stats
{
    public interface IStatsEnricher
    {
        Repository Enrich(Repository repository,
                          RepositoryStatsEnricherArguments args);

        RepositorySnapshot EnrichWithCoverage(RepositorySnapshot repositorySnapshot);

        RepositorySnapshot EnrichWithGitStats(RepositorySnapshot repositorySnapshot,
                                              GitStatGranularity granularity = GitStatGranularity.File);

        File Enrich(File file, string coverageReportPath, string coverageCommand);
    }
}
