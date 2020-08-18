using System.Collections.Generic;
using System.Linq;
using Hestia.DAL.EFCore.Entities;
using Hestia.DAL.Interfaces;

namespace Hestia.DAL.EFCore.Adapters
{
    public class FileEntityAdapter : IFileEntity
    {
        private readonly FileEntity _entity;

        public FileEntityAdapter(FileEntity entity) => _entity = entity;

        public string Id => _entity.Id;

        public string Path => _entity.Path;

        public decimal CoveragePercentage => _entity.CoveragePercentage;

        public int LifetimeAuthors => _entity.LifetimeAuthors;

        public int LifetimeChanges => _entity.LifetimeChanges;

        public IEnumerable<ILineEntity> Lines => _entity.Lines
                                                 .Select(AdapterExtensions.AsModel);
    }
}
