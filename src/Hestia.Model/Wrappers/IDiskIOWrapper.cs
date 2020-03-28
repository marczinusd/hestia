using System.Collections.Generic;

namespace Hestia.Model.Wrappers
{
    public interface IDiskIOWrapper
    {
        SourceLine[] ReadAllLinesFromFileAsSourceModel(string filePath);

        IEnumerable<string> ReadAllLinesFromFile(string filePath);

        IEnumerable<string> EnumerateAllFilesForPathRecursively(string path);

        string ReadFileContent(string path);

        bool FileExists(string path);
    }
}
