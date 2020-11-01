﻿using Hestia.DAL.EFCore.Entities;
using Hestia.DAL.Interfaces;

namespace Hestia.DAL.EFCore.Adapters
{
    public class LineEntityAdapter : ILineEntity
    {
        private readonly Line _entity;

        public LineEntityAdapter(Line entity) => _entity = entity;

        public string Content => _entity.Content;

        public bool IsCovered => _entity.IsCovered;

        public bool IsBranched => _entity.IsBranched;

        public string ConditionCoverage => _entity.ConditionCoverage;

        public int HitCount => _entity.HitCount;

        public int NumberOfAuthors => _entity.NumberOfAuthors;

        public int NumberOfChanges => _entity.NumberOfChanges;

        public int LineNumber => _entity.LineNumber;
    }
}
