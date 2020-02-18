using System;
using System.Collections.Generic;
using IO = System.IO;

namespace Hestia.Model.Wrappers
{
    public class DiskIOWrapper : IDiskIOWrapper
    {
        public SourceLine[] ReadAllLinesFromFileAsSourceModel(string filePath)
        {
            Console.WriteLine(filePath);

            throw new NotImplementedException();
        }

        public IEnumerable<string> ReadAllLinesFromFile(string filePath) => IO.File.ReadAllLines(filePath);

        public IEnumerable<string> EnumerateAllDirectoriesForPath(string path) => IO.Directory.GetDirectories(path);

        public IEnumerable<string> EnumerateAllFilesForPath(string path) => IO.Directory.GetFiles(path);
    }
}
