using System;

namespace Hestia.Model.Interfaces
{
    public interface ISnapshotHeader
    {
        string Id { get; }

        string Name { get; }

        string AtHash { get; }

        DateTime? CommitDate { get; }
    }
}
