using System.Collections.Generic;

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

        public IEnumerable<SourceLine> Content => _file.Content;

        public string Filename => _file.Filename;

        public string Extension => _file.Extension;
    }
}
