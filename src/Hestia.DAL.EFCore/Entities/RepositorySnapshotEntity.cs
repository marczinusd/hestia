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

        public IEnumerable<FileEntity> Files { get; set; }

        public string Id { get; set; }

        public string Name { get; set; }

        public string AtHash { get; set; }

        public DateTime? CommitDate { get; set; }
    }
}
