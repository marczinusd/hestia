using System;
using System.Collections.Generic;
using System.Linq;
using Hestia.DAL.EFCore.Entities;
using Hestia.DAL.Interfaces;

namespace Hestia.DAL.EFCore.Adapters
{
    public class RepositorySnapshotEntityAdapter : IRepositorySnapshotEntity
    {
        private readonly RepositorySnapshotEntity _entity;

        public RepositorySnapshotEntityAdapter(RepositorySnapshotEntity entity) => _entity = entity;

        public string Id => _entity.Id;

        public string Name => _entity.Name;

        public string AtHash => _entity.AtHash;

        public DateTime? CommitDate => _entity.CommitDate;

        public IEnumerable<IFileEntity> Files => _entity.Files
                                                        .Select(AdapterExtensions.AsModel)
                                                        .ToList();
    }
}
