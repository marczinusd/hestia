using System;
using System.Runtime.Serialization;

namespace Hestia.Model.Builders
{
    // ReSharper disable once RedundantExtendsListEntry
    [Serializable]
    public class FileBuilderException : Exception
    {
        public FileBuilderException(string filePath)
            : base($"File at {filePath} is invalid")
        {
        }
    }
}
