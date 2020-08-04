using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Xml.Serialization;

namespace Hestia.Model.Wrappers
{
    [ExcludeFromCodeCoverage]
    public class XmlFileSerializationWrapper : IXmlFileSerializationWrapper
    {
        public T Deserialize<T>(string path, FileMode fileMode)
        {
            // Creating the serializer like this is less than ideal due to the perf overhead of xml serializer creation
            // Refactor if this is ever used in multiple places
            var serializer = new XmlSerializer(typeof(T));
            using Stream reader = new FileStream(path, fileMode);

            return (T)serializer.Deserialize(reader);
        }
    }
}
