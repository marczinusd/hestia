using FluentAssertions;
using Hestia.DAL.EFCore.Entities;
using Xunit;

namespace Test.Hestia.DAL.EFCore
{
    public class EntityAxioms
    {
        [Fact]
        public void LineEntityHasSettableParentLinks()
        {
            var entity = new LineEntity { Parent = new FileEntity() };

            entity.Parent.Should()
                  .NotBeNull();
        }

        [Fact]
        public void FileEntityHasSettableParentLinks()
        {
            var entity = new FileEntity { Parent = new RepositorySnapshotEntity() };

            entity.Parent.Should()
                  .NotBeNull();
        }

        [Fact]
        public void EntitiesHaveParameterlessConstructors()
        {
            var line = new LineEntity();
            var file = new FileEntity();
            var snapshot = new RepositorySnapshotEntity();

            line.Should()
                .NotBeNull();
            file.Should()
                .NotBeNull();
            snapshot.Should()
                    .NotBeNull();
        }
    }
}
