using System;
using Hestia.Model.Interfaces;

namespace Hestia.WebService.ServiceModel
{
    public class SlimSnapshot : ISnapshotHeader
    {
        private SlimSnapshot(ISnapshotHeader maybeEntity)
        {
            Id = maybeEntity.Id;
            Name = maybeEntity.Name;
            AtHash = maybeEntity.AtHash;
            CommitDate = maybeEntity.CommitDate;
        }

        public string Id { get; }

        public string Name { get; }

        public string AtHash { get; }

        public DateTime? CommitDate { get; }

        public static ISnapshotHeader From(ISnapshotHeader maybeEntity) => new SlimSnapshot(maybeEntity);
    }
}
