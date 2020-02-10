using System.Collections.Generic;

namespace Hestia.Model
{
    public class File
    {
        public File(IEnumerable<SourceLine> content, string path, string filename, string extension)
        {
            Content = content;
            Path = path;
            Filename = filename;
            Extension = extension;
        }

        public IEnumerable<SourceLine> Content { get; }

        public string Path { get; }

        public string Filename { get; }

        public string Extension { get; }
    }
}
