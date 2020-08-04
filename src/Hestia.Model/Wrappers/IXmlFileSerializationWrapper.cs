using System.IO;

namespace Hestia.Model.Wrappers
{
    public interface IXmlFileSerializationWrapper
    {
        T Deserialize<T>(string path, FileMode fileMode);
    }
}
