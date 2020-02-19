using System.Collections.Generic;
using System.Text.Json.Serialization;
using Hestia.Model.Stats;
using LanguageExt;

namespace Hestia.Model
{
    public class File
    {
        public File(long id,
                    string filename,
                    string extension,
                    string path,
                    IList<SourceLine> content,
                    Option<FileGitStats> gitStats,
                    Option<FileCoverageStats> coverageStats)
        {
            Id = id;
            Content = content;
            Path = path;
            Filename = filename;
            Extension = extension;
            GitStats = gitStats;
            CoverageStats = coverageStats;
        }

        public long Id { get; }

        [JsonIgnore]
        public IList<SourceLine> Content { get; }

        public string Path { get; }

        public string Filename { get; }

        public string Extension { get; }

        public Option<FileGitStats> GitStats { get; }

        public Option<FileCoverageStats> CoverageStats { get; }

        public FileDetails AsFileDetails()
        {
            var hardCopy = new File(0, this.Filename, this.Extension, this.Path, Content, GitStats, CoverageStats);

            return new FileDetails(hardCopy);
        }
    }
}
