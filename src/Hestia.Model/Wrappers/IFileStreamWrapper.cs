using System.IO;

namespace Hestia.Model.Wrappers
{
    public interface IFileStreamWrapper
    {
        T Deserialize<T>(string path, FileMode fileMode);
    }
}
