using LanguageExt;

namespace Hestia.DAL.Interfaces
{
    public interface IFileRetrieval
    {
        Option<IFileEntity> GetFileDetails(string fileId, string snapshotId);
    }
}
