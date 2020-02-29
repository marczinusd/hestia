using System;
using System.Collections.Generic;

namespace Hestia.Model.Wrappers
{
    public class DiskIOWrapper : IDiskIOWrapper
    {
        public SourceLine[] ReadAllLinesFromFileAsSourceModel(string filePath)
        {
            Console.WriteLine(filePath);

            throw new NotImplementedException();
        }

        public IEnumerable<string> ReadAllLinesFromFile(string filePath) => System.IO.File.ReadAllLines(filePath);

        public IEnumerable<string> EnumerateAllDirectoriesForPath(string path) =>
            System.IO.Directory.GetDirectories(path);

        public IEnumerable<string> EnumerateAllFilesForPath(string path) => System.IO.Directory.GetFiles(path);

        public string ReadFileContent(string path) => System.IO.File.ReadAllText(path);
    }
}
