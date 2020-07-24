using System;
using System.Runtime.Serialization;

namespace Hestia.Model.Builders
{
    // ReSharper disable once RedundantExtendsListEntry
    public class FileBuilderException : Exception, ISerializable
    {
        public FileBuilderException(string filePath)
            : base($"File at {filePath} is invalid")
        {
        }

        // ReSharper disable once UnusedMember.Global
        protected FileBuilderException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {
        }
    }
}
