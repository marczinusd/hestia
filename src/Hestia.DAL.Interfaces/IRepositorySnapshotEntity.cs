using System;
using System.Collections.Generic;

namespace Hestia.DAL.Interfaces
{
    public interface IRepositorySnapshotEntity
    {
        IEnumerable<IFileEntity> Files { get; }

        string AtHash { get; }

        DateTime? HashDate { get; }
    }
}
