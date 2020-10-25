using System.Collections.Generic;
using LanguageExt;

namespace Hestia.DAL.Interfaces
{
    public interface IFileRetrieval
    {
        Option<IFileEntity> GetFileDetails(string fileId);

        IEnumerable<ILineEntity> GetLinesForFile(string fileId);

        bool FileExistsWithId(string fileId);
    }
}
