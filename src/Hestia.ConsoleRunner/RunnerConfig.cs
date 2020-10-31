using System.Collections.Generic;
using JetBrains.Annotations;

namespace Hestia.ConsoleRunner
{
    public class RunnerConfig
    {
        [UsedImplicitly]
        public string CoverageReportLocation { get; set; }

        [UsedImplicitly]
        public string RepoRoot { get; set; }

        [UsedImplicitly]
        public string SourceRelativePath { get; set; }

        [UsedImplicitly]
        public List<string> FileExtensions { get; set; }

        [UsedImplicitly]
        public string SqliteDbLocation { get; set; }

        [UsedImplicitly]
        public string SqliteDbName { get; set; }

        [UsedImplicitly]
        public string StatGranularity { get; set; }
    }
}
