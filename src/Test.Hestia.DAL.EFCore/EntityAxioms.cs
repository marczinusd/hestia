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
            var entity = new Line { File = new File() };

            entity.File.Should()
                  .NotBeNull();
        }

        [Fact]
        public void FileEntityHasSettableParentLinks()
        {
            var entity = new File { Snapshot = new Snapshot() };

            entity.Snapshot.Should()
                  .NotBeNull();
        }

        [Fact]
        public void EntitiesHaveParameterlessConstructors()
        {
            var line = new Line();
            var file = new File();
            var snapshot = new Snapshot();

            line.Should()
                .NotBeNull();
            file.Should()
                .NotBeNull();
            snapshot.Should()
                    .NotBeNull();
        }
    }
}
