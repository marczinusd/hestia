using System.Collections.Generic;
using System.Text.Json.Serialization;
using Hestia.Model.Interfaces;
using JetBrains.Annotations;
using LanguageExt;
using static LanguageExt.Prelude;

namespace Hestia.Model
{
    public class File : IFile
    {
        public File(string filename,
                    string extension,
                    string path,
                    IList<ISourceLine> content,
                    Option<IFileGitStats> gitStats,
                    Option<IFileCoverageStats> coverageStats)
        {
            Content = content;
            Path = path;
            Filename = filename;
            Extension = extension;
            GitStats = gitStats;
            CoverageStats = coverageStats;
        }

        [JsonIgnore] public IList<ISourceLine> Content { get; }

        [JsonIgnore] public string FullPath => System.IO.Path.Join(Path, Filename);

        public string Path { get; }

        public string Filename { get; }

        public string Extension { get; }

        public Option<IFileGitStats> GitStats { get; }

        public Option<IFileCoverageStats> CoverageStats { get; }

        [JsonIgnore]
        [UsedImplicitly]
        public decimal CoveragePercentage => CoverageStats.Match(x => x.PercentageOfLineCoverage, -1);

        [JsonIgnore] [UsedImplicitly] public int LifetimeAuthors => GitStats.Match(x => x.LifetimeAuthors, -1);

        [JsonIgnore] [UsedImplicitly] public int LifetimeChanges => GitStats.Match(x => x.LifetimeChanges, -1);

        public IFile With(IList<ISourceLine>? content = null,
                          IFileGitStats? gitStats = null,
                          IFileCoverageStats? coverageStats = null) =>
            new File(Filename,
                     Extension,
                     Path,
                     content ?? Content,
                     gitStats != null ? Some(gitStats) : GitStats,
                     coverageStats != null ? Some(coverageStats) : CoverageStats);

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
