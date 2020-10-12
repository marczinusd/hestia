using System.Collections.Generic;
using Hestia.Model.Interfaces;

namespace Hestia.Model.Wrappers
{
    public interface IDiskIOWrapper
    {
        ISourceLine[] ReadAllLinesFromFileAsSourceModel(string filePath);

        IEnumerable<string> ReadAllLinesFromFile(string filePath);

        IEnumerable<string> EnumerateAllFilesForPathRecursively(string path);

        string ReadFileContent(string path);

        bool FileExists(string path);

        bool DirectoryExists(string path);
    }
}
