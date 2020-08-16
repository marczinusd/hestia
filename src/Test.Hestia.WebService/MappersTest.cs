using FluentAssertions;
using Hestia.DAL.EFCore.Entities;
using Hestia.WebService.Helpers;
using Xunit;

namespace Test.Hestia.WebService
{
    public class MappersTest
    {
        [Fact]
        public void EntityAsHeaderMapsEntityAsExpected()
        {
            var entity = new FileEntity("path",
                                        1,
                                        2,
                                        3);

            var result = Mappers.EntityAsHeader(entity);

            result.Path
                  .Should()
                  .Be(entity.Path);
            result.CoveragePercentage
                  .Should()
                  .Be(entity.CoveragePercentage);
            result.LifetimeAuthors
                  .Should()
                  .Be(entity.LifetimeAuthors);
            result.LifetimeChanges
                  .Should()
                  .Be(entity.LifetimeChanges);
        }
    }
}
