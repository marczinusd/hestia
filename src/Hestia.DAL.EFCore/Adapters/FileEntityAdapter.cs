﻿using System.Collections.Generic;
using System.Linq;
using Hestia.DAL.EFCore.Entities;
using Hestia.DAL.Interfaces;

namespace Hestia.DAL.EFCore.Adapters
{
    public class FileEntityAdapter : IFileEntity
    {
        private readonly File _entity;

        public FileEntityAdapter(File entity)
        {
            _entity = entity;
            Lines = entity.Lines?
                          .Select(AdapterExtensions.AsModel)
                          .ToList() ?? new List<ILineEntity>();
        }

        public string Id => _entity.Id;

        public string Path => _entity.Path;

        public decimal CoveragePercentage => _entity.CoveragePercentage;

        public int LifetimeAuthors => _entity.LifetimeAuthors;

        public int LifetimeChanges => _entity.LifetimeChanges;

        // this is incredibly ugly, but due to the EFCore weirdness that I don't have to fix
        // it'll have to stay so I don't pollute entities with manual joins
        public List<ILineEntity> Lines { get; set; }
    }
}
