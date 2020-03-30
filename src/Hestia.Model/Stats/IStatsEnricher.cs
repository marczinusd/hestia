namespace Hestia.Model.Stats
{
    public interface IStatsEnricher
    {
        Repository Enrich(Repository repository,
                          RepositoryStatsEnricherArguments args);

        RepositorySnapshot EnrichWithCoverage(RepositorySnapshot repositorySnapshot);

        RepositorySnapshot EnrichWithGitStats(RepositorySnapshot repositorySnapshot);

        File Enrich(File file, string coverageReportPath, string coverageCommand);
    }
}
