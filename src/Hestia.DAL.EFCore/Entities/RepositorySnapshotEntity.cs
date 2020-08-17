using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Hestia.DAL.EFCore.Entities
{
    public class RepositorySnapshotEntity
    {
        [UsedImplicitly]
        public RepositorySnapshotEntity()
        {
        }

        public RepositorySnapshotEntity(IEnumerable<FileEntity> files,
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

        [UsedImplicitly] public IEnumerable<FileEntity> Files { get; set; }

        [UsedImplicitly] public string Id { get; set; }

        [UsedImplicitly] public string Name { get; set; }

        [UsedImplicitly] public string AtHash { get; set; }

        [UsedImplicitly] public DateTime? CommitDate { get; set; }
    }
}
