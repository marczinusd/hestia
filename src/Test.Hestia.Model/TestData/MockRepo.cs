using System.IO;
using Hestia.Model.Wrappers;
using Moq;

namespace Test.Hestia.Model.TestData
{
    public static class MockRepo
    {
        private static readonly string DirPath = Path.Join("C:", "temp");

        public static Mock<IDiskIOWrapper> CreateDiskIOWrapperMock()
        {
            var ioWrapper = new Mock<IDiskIOWrapper>();
            ioWrapper.Setup(mock => mock.EnumerateAllFilesForPathRecursively(It.IsAny<string>()))
                     .Returns(new[]
                     {
                         Path.Join(DirPath, "bla2.js"),
                         Path.Join(DirPath, "model/File.cs"),
                         Path.Join(DirPath, "model/something/bla2.cs"),
                         Path.Join(DirPath, "server/bla2.js"),
                     });

            return ioWrapper;
        }
    }
}
