using System.Linq;
using FluentAssertions;
using Hestia.Model;
using Xunit;

namespace Test.Hestia.Model
{
    public class DirectoryBuilderTest
    {
        [Fact]
        public void EmptyDirectoryTest()
        {
            using (new FileSystemTestContext(Enumerable.Empty<string>()))
            {
                var dir = DirectoryBuilder.BuildDirectoryStructureFromFilePath("./");

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
