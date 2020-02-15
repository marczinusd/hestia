using System;

namespace Hestia.Model.Wrappers
{
    public class DiskIOWrapper : IDiskIOWrapper
    {
        public SourceLine[] ReadAllLinesFromFile(string filePath)
        {
            Console.WriteLine(filePath);

            throw new NotImplementedException();
        }
    }
}
