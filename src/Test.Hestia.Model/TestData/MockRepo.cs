using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Hestia.Model;
using Hestia.Model.Builders;
using Hestia.Model.Wrappers;
using LanguageExt;
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
                         Path.Join(DirPath, "bla2.js"), Path.Join(DirPath, "model/File.cs"),
                         Path.Join(DirPath, "model/something/bla2.cs"),
                         Path.Join(DirPath, "server/bla2.js"),
                     });

            return ioWrapper;
        }

        public static RepositorySnapshot CreateSnapshot(IEnumerable<string> extensions,
                                                        string coveragePath,
                                                        IDiskIOWrapper ioWrapper,
                                                        IPathValidator validator) =>
            new RepositorySnapshotBuilderArguments(1,
                                                   DirPath,
                                                   string.Empty,
                                                   extensions.ToArray(),
                                                   coveragePath,
                                                   Option<string>.None,
                                                   Option<DateTime>.None,
                                                   ioWrapper,
                                                   validator).Build();
    }
}
