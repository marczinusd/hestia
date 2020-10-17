﻿using System;
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

        public RepositorySnapshotEntity(IList<FileEntity> files,
                                        string atHash,
                                        DateTime? hashDate,
                                        string name,
                                        string id,
                                        int numberOfCommits,
                                        int commitRelativePosition)
        {
            Files = files;
            AtHash = atHash;
            CommitDate = hashDate;
            Name = name;
            Id = id;
            NumberOfCommits = numberOfCommits;
            CommitRelativePosition = commitRelativePosition;
        }

        [UsedImplicitly] public IList<FileEntity> Files { get; set; }

        [UsedImplicitly] public string Id { get; set; }

        [UsedImplicitly] public string Name { get; set; }

        [UsedImplicitly] public string AtHash { get; set; }

        [UsedImplicitly] public DateTime? CommitDate { get; set; }

        [UsedImplicitly] public int NumberOfCommits { get; set; }

        [UsedImplicitly] public int CommitRelativePosition { get; set; }
    }
}
