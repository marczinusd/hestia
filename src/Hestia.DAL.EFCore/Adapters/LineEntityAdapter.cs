using Hestia.DAL.EFCore.Entities;
using Hestia.DAL.Interfaces;

namespace Hestia.DAL.EFCore.Adapters
{
    public class LineEntityAdapter : ILineEntity
    {
        private readonly LineEntity _entity;

        public LineEntityAdapter(LineEntity entity) => _entity = entity;

        public string Content => _entity.Content;

        public bool IsCovered => _entity.IsCovered;

        public int NumberOfAuthors => _entity.NumberOfAuthors;

        public int NumberOfChanges => _entity.NumberOfChanges;
    }
}
