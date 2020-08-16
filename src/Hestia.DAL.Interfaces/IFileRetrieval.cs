namespace Hestia.DAL.Interfaces
{
    public interface IFileRetrieval
    {
        IFileEntity GetFileDetails(string fileId, string snapshotId);
    }
}
