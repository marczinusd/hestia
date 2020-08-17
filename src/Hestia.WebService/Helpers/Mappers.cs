using Hestia.DAL.Interfaces;
using Hestia.Model;
using Hestia.Model.Interfaces;

namespace Hestia.WebService.Helpers
{
    public static class Mappers
    {
        public static IFileHeader EntityAsHeader(IFileEntity entity)
            => new FileHeader(entity.Path,
                              entity.CoveragePercentage,
                              entity.LifetimeAuthors,
                              entity.LifetimeChanges,
                              entity.Id);
    }
}
