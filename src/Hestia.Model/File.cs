using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Hestia.Model.Stats;
using LanguageExt;

namespace Hestia.Model
{
    public class File
    {
        public File(string filename,
                    string extension,
                    string path,
                    IEnumerable<SourceLine> content,
                    Option<FileGitStats> gitStats,
                    Option<FileCoverageStats> coverageStats)
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

        [NotMapped] public Option<FileGitStats> GitStats { get; }

        [NotMapped] public Option<FileCoverageStats> CoverageStats { get; }
    }
}
