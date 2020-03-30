using System.Linq;
using FluentAssertions;
using Hestia.Model.Wrappers;
using Hestia.UIRunner.ViewModels;
using Moq;
using Xunit;

namespace Test.Hestia.UIRunner
{
    public class RepositoryFormVIewModelTest
    {
        [Fact]
        public void RepositoryPathShouldPointToDirectoryThatExists()
        {
            const string path = "somePath";
            var ioMock = new Mock<IDiskIOWrapper>();
            ioMock.Setup(mock => mock.DirectoryExists(path))
                  .Returns(false);
            var vm = new RepositoryFormViewModel(ioMock.Object) { RepositoryPath = path };

            vm.ValidationContext.Text
              .First()
              .Should()
              .Contain("Directory does not exist");
        }
    }
}
