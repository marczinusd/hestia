using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Hestia.Model.Stats;
using LanguageExt;

namespace Hestia.Model
{
    public class File
    {
        // EFCore needs parameterless ctors for entities
        // ReSharper disable once UnusedMember.Global
        public File()
        {
        }

        public File(long id,
                    string filename,
                    string extension,
                    string path,
                    IEnumerable<SourceLine> content,
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

        public long Id { get; set; }

        [JsonIgnore]
        public IEnumerable<SourceLine> Content { get; set; }

        public string Path { get; set; }

        public string Filename { get; set; }

        public string Extension { get; set; }

        [NotMapped] public Option<FileGitStats> GitStats { get; set; }

        [NotMapped] public Option<FileCoverageStats> CoverageStats { get; set; }

        public FileDetails AsFileDetails()
        {
            var hardCopy = new File(0, this.Filename, this.Extension, this.Path, Content, GitStats, CoverageStats);

            return new FileDetails(hardCopy);
        }
    }
}
