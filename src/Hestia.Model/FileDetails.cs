using System.Collections.Generic;
using Hestia.Model.Interfaces;
using LanguageExt;

namespace Hestia.Model
{
    /// <summary>
    ///     Exposes the full details of a file to the JSON API.
    /// </summary>
    public class FileDetails
    {
        private readonly File _file;

        public FileDetails(File file)
        {
            _file = file;
        }

        public IList<ISourceLine> Content => _file.Content;

        public string Filename => _file.Filename;

        public string Extension => _file.Extension;

        public string Path => _file.Path;

        public Option<IFileGitStats> GitStats => _file.GitStats;

        public Option<IFileCoverageStats> CoverageStats => _file.CoverageStats;

        public File AsSlimFile() =>
            new File(Filename,
                     Extension,
                     Path,
                     Content,
                     GitStats,
                     CoverageStats);
    }
}
