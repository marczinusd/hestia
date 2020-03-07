using System.Collections.Generic;
using System.Text.Json.Serialization;
using Hestia.Model.Stats;
using LanguageExt;

namespace Hestia.Model
{
    public class File
    {
        public File(string filename,
                    string extension,
                    string path,
                    IList<SourceLine> content,
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

        [JsonIgnore] public IList<SourceLine> Content { get; }

        [JsonIgnore] public string FullPath => System.IO.Path.Join(Path, Filename);

        public string Path { get; }

        public string Filename { get; }

        public string Extension { get; }

        public Option<FileGitStats> GitStats { get; }

        public Option<FileCoverageStats> CoverageStats { get; }

        public File With(IList<SourceLine>? content = null,
                         FileGitStats? gitStats = null,
                         FileCoverageStats? coverageStats = null) =>
            new File(Filename,
                     Extension,
                     Path,
                     content ?? Content,
                     gitStats ?? GitStats,
                     coverageStats ?? CoverageStats);

        public FileDetails AsFileDetails()
        {
            var hardCopy = new File(this.Filename,
                                    this.Extension,
                                    this.Path,
                                    Content,
                                    GitStats,
                                    CoverageStats);

            return new FileDetails(hardCopy);
        }
    }
}
