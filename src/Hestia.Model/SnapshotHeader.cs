using System;
using Hestia.Model.Interfaces;

namespace Hestia.Model
{
    public class SnapshotHeader : ISnapshotHeader
    {
        public SnapshotHeader(string id, string name, string atHash, DateTime commitDate)
        {
            Id = id;
            Name = name;
            AtHash = atHash;
            CommitDate = commitDate;
        }

        public string Id { get; }

        public string Name { get; }

        public string AtHash { get; }

        public DateTime? CommitDate { get; }
    }
}
