using System.IO;
using Xunit;

namespace Test.Hestia.Model.Builders
{
    public class DirectoryBuilderTest
    {
        private static string dirPath = Path.Join("C:", "temp");
        private static string[] rootDirs = { Path.Join(dirPath, "firstDir"), Path.Join("secondDir") };

        [Fact]
        public void EmptyDirectoryTest()
        {
        }
    }
}
