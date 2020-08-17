using System;
using System.Collections.Generic;
using Hestia.DAL.Interfaces;
using Hestia.Model.Interfaces;

namespace Hestia.DAL.EFCore.Entities
{
    public class RepositorySnapshotEntity : IRepositorySnapshotEntity
    {
        public RepositorySnapshotEntity(IEnumerable<IFileEntity> files,
                                        string atHash,
                                        DateTime? hashDate,
                                        string name,
                                        string id)
        {
            Files = files;
            AtHash = atHash;
            CommitDate = hashDate;
            Name = name;
            Id = id;
        }

        public IEnumerable<IFileEntity> Files { get; }

        public string Id { get; }

        public string Name { get; }

        public string AtHash { get; }

        public DateTime? CommitDate { get; }
    }
}
