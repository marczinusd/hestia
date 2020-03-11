using System.Collections.Generic;

namespace Hestia.Model.Wrappers
{
    public interface IDiskIOWrapper
    {
        SourceLine[] ReadAllLinesFromFileAsSourceModel(string filePath);

        IEnumerable<string> ReadAllLinesFromFile(string filePath);

        IEnumerable<string> EnumerateAllDirectoriesForPath(string path);

        IEnumerable<string> EnumerateAllFilesForPath(string path);

        IEnumerable<string> EnumerateAllFilesForPathRecursively(string path);

        string ReadFileContent(string path);
    }
}
