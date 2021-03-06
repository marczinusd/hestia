﻿using System.Collections.Generic;
using Hestia.Model.Interfaces;

namespace Hestia.DAL.Interfaces
{
    public interface IRepositorySnapshotEntity : ISnapshotHeader
    {
        IEnumerable<IFileEntity> Files { get; }

        int? NumberOfCommits { get; }

        int? CommitRelativePosition { get; }
    }
}
