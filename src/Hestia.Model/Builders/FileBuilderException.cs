using System;

namespace Hestia.Model.Builders
{
    public class FileBuilderException : Exception
    {
        public FileBuilderException(string filePath)
            : base($"File at {filePath} is invalid")
        {
        }
    }
}
