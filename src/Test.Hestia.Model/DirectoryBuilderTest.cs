using System.Linq;
using FluentAssertions;
using Hestia.Model.Builders;
using Hestia.Model.Wrappers;

namespace Test.Hestia.Model
{
    public class DirectoryBuilderTest
    {
        // [Fact]
        public void EmptyDirectoryTest()
        {
            using (new FileSystemTestContext(Enumerable.Empty<string>()))
            {
                var dir = DirectoryBuilder.BuildDirectoryFromDirectoryPath("./", new DiskIOWrapper());

                dir.Directories
                   .Should()
                   .BeEmpty();
                dir.Files
                   .Should()
                   .BeEmpty();
            }
        }
    }
}
