using System.Collections.Generic;
using Hestia.Model.Stats;
using LanguageExt;

namespace Hestia.Model
{
    public class File
    {
        public File(IEnumerable<SourceLine> content, string path, string filename, string extension, Option<FileGitStats> gitStats, Option<FileCoverageStats> coverageStats)
        {
            Content = content;
            Path = path;
            Filename = filename;
            Extension = extension;
            GitStats = gitStats;
            CoverageStats = coverageStats;
        }

        public IEnumerable<SourceLine> Content { get; }

        public string Path { get; }

        public string Filename { get; }

        public string Extension { get; }

        public Option<FileGitStats> GitStats { get; }

        public Option<FileCoverageStats> CoverageStats { get; }
    }
}
