using System.Collections.Generic;
using FluentAssertions;
using Hestia.DAL.EFCore.Entities;
using Hestia.DAL.Interfaces;
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
                                        3,
                                        new List<ISourceLineEntity>()
                                        {
                                            new LineEntity("bla",
                                                           true,
                                                           2,
                                                           3),
                                        },
                                        "id");

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
            result.Id
                  .Should()
                  .BeNull();
        }
    }
}
