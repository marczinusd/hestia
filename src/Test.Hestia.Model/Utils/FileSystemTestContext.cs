using System;
using System.Collections.Generic;
using System.IO;

namespace Test.Hestia.Model.Utils
{
    public class FileSystemTestContext : IDisposable
    {
        private readonly IEnumerable<string> _filesToCreate;

        public FileSystemTestContext(IEnumerable<string> filesToCreate)
        {
            _filesToCreate = filesToCreate;
            foreach (var s in _filesToCreate)
            {
                File.Create(s)
                    .Close();
            }
        }

        public void Dispose()
        {
            foreach (var s in _filesToCreate)
            {
                File.Delete(s);
            }
        }
    }
}
