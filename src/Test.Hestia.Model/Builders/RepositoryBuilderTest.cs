using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using Hestia.Model.Builders;
using Hestia.Model.Wrappers;
using LanguageExt;
using Moq;
using Xunit;

namespace Test.Hestia.Model.Builders
{
    public class RepositoryBuilderTest
    {
        private static readonly string DirPath = Path.Join("C:", "temp");

        [Fact]
        public void RepositoryBuilderValidatesThatDirectoryIsAGitRepository()
        {
            var ioWrapper = new Mock<IDiskIOWrapper>();
            var validator = new Mock<IPathValidator>();
            ioWrapper.Setup(mock => mock.EnumerateAllDirectoriesForPath(DirPath))
                     .Returns(Enumerable.Empty<string>());
            var args = new RepositorySnapshotBuilderArguments(-1,
                                                              string.Empty,
                                                              DirPath,
                                                              Array.Empty<string>(),
                                                              string.Empty,
                                                              Option<string>.None,
                                                              Option<DateTime>.None,
                                                              ioWrapper.Object,
                                                              validator.Object);

            Action act = () => args.Build();

            act.Should()
               .Throw<InvalidOperationException>();
        }
    }
}
