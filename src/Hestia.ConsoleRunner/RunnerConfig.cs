using System.Collections.Generic;

namespace Hestia.ConsoleRunner
{
    public class RunnerConfig
    {
        public string CoverageReportLocation { get; set; }

        public string RepoRoot { get; set; }

        public string SourceRelativePath { get; set; }

        public List<string> FileExtensions { get; set; }

        public string SqliteDbLocation { get; set; }
    }
}
