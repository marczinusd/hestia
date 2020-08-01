using System;
using Hestia.DAL.Interfaces;
using Hestia.Model.Interfaces;

namespace Hestia.DAL.EFCore.Entities
{
    public static class Extensions
    {
        public static IFileEntity AsEntity(this IFile file) => throw new NotImplementedException();

        public static IRepositorySnapshotEntity AsEntity(this IRepositorySnapshot snapshot) => throw new NotImplementedException();
    }
}
