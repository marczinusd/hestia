namespace Hestia.Model.Stats
{
    public class RepositoryStatsEnricherArguments
    {
        public RepositoryStatsEnricherArguments(string repoPath,
                                                string sourceRoot,
                                                string[] sourceExtensions,
                                                string coverageCommand,
                                                string coverageOutputLocation,
                                                int firstCommitToSample,
                                                int lastCommitToSample,
                                                int numberOfSamples)
        {
            RepoPath = repoPath;
            SourceRoot = sourceRoot;
            SourceExtensions = sourceExtensions;
            CoverageCommand = coverageCommand;
            CoverageOutputLocation = coverageOutputLocation;
            FirstCommitToSample = firstCommitToSample;
            LastCommitToSample = lastCommitToSample;
            NumberOfSamples = numberOfSamples;
        }

        public string RepoPath { get; }

        public string SourceRoot { get; }

        public string[] SourceExtensions { get; }

        public string CoverageCommand { get; }

        public string CoverageOutputLocation { get; }

        public int FirstCommitToSample { get; }

        public int LastCommitToSample { get; }

        public int NumberOfSamples { get; }
    }
}
