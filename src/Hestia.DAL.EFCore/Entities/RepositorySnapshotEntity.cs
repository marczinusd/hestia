using System;
using System.Collections.Generic;
using Hestia.DAL.Interfaces;

namespace Hestia.DAL.EFCore.Entities
{
    public class RepositorySnapshotEntity : IRepositorySnapshotEntity
    {
        public RepositorySnapshotEntity(IEnumerable<IFileEntity> files, string atHash, DateTime? hashDate)
        {
            Files = files;
            AtHash = atHash;
            HashDate = hashDate;
        }

        public IEnumerable<IFileEntity> Files { get; }

        public string AtHash { get; }

        public DateTime? HashDate { get; }
    }
}
