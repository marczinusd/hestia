using System.Collections.Generic;
using Hestia.Model.Interfaces;

namespace Hestia.DAL.Interfaces
{
    public interface IFileEntity : IFileHeader
    {
        List<ILineEntity> Lines { get; }
    }
}
