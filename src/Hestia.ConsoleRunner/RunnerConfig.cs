using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace Hestia.ConsoleRunner
{
    [ExcludeFromCodeCoverage]
    public class RunnerConfig
    {
        [UsedImplicitly]
        [JsonPropertyName("coverageReportLocation")]
        public string CoverageReportLocation { get; set; }

        [UsedImplicitly]
        [JsonPropertyName("repoRoot")]
        public string RepoRoot { get; set; }

        [UsedImplicitly]
        [JsonPropertyName("sourceRelativePath")]
        public string SourceRelativePath { get; set; }

        [UsedImplicitly]
        [JsonPropertyName("fileExtensions")]
        public List<string> FileExtensions { get; set; }

        [UsedImplicitly]
        [JsonPropertyName("sqliteDbLocation")]
        public string SqliteDbLocation { get; set; }

        [UsedImplicitly]
        [JsonPropertyName("sqliteDbName")]
        public string SqliteDbName { get; set; }

        [UsedImplicitly]
        [JsonPropertyName("statGranularity")]
        public string StatGranularity { get; set; }

        [UsedImplicitly]
        [JsonPropertyName("ignorePatterns")]
        public List<string> IgnorePatterns { get; set; }

        [UsedImplicitly]
        [JsonPropertyName("buildCommands")]
        public IList<string> BuildCommands { get; set; }

        [UsedImplicitly]
        [JsonPropertyName("testCommands")]
        public IList<string> TestCommands { get; set; }
    }
}
