using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using Hestia.Model.Builders;
using Hestia.Model.Wrappers;
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

            Action act = () => RepositoryBuilder.BuildRepositoryFromDirectoryPath(-1,
                                                                                  DirPath,
                                                                                  ioWrapper.Object,
                                                                                  validator.Object);

            act.Should()
               .Throw<InvalidOperationException>();
        }
    }
}
