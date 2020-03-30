using System.IO;
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
        private const string RepoPath = "somePath";

        [Fact]
        public void RepositoryPathShouldPointToDirectoryThatExists()
        {
            var ioMock = new Mock<IDiskIOWrapper>();
            ioMock.Setup(mock => mock.DirectoryExists(RepoPath))
                  .Returns(false);

            var vm = new RepositoryFormViewModel(ioMock.Object) { RepositoryPath = RepoPath };

            vm.ValidationContext.Text
              .First()
              .Should()
              .Contain("Directory does not exist");
        }

        [Fact]
        public void RepositoryPathShouldPointToGitRepository()
        {
            var ioMock = new Mock<IDiskIOWrapper>();
            ioMock.Setup(mock => mock.DirectoryExists(RepoPath))
                  .Returns(true);
            ioMock.Setup(mock => mock.DirectoryExists(Path.Join(RepoPath, ".git")))
                  .Returns(false);

            var vm = new RepositoryFormViewModel(ioMock.Object) { RepositoryPath = RepoPath };

            vm.ValidationContext.Text
              .First()
              .Should()
              .Contain("Directory is not a git repository");
        }

        [Fact]
        public void RepositoryPathEmptyFieldValidation()
        {
            var vm = new RepositoryFormViewModel(new DiskIOWrapper())
            {
                CoverageCommand = "bla",
                SourceExtensions = "bla",
                CoverageOutputLocation = "bla",
            };

            vm.ValidationContext.Text
              .Any(s => s.Contains("RepositoryPath should not be empty"))
              .Should()
              .BeTrue();
        }

        [Fact]
        public void CoverageCommandEmptyFieldValidation()
        {
            var vm = new RepositoryFormViewModel(new DiskIOWrapper())
            {
                RepositoryPath = "bla",
                SourceExtensions = "bla",
                CoverageOutputLocation = "bla",
            };

            vm.ValidationContext.Text
              .Any(s => s.Contains("CoverageCommand should not be empty"))
              .Should()
              .BeTrue();
        }

        [Fact]
        public void CoverageOutputLocationEmptyFieldValidation()
        {
            var vm = new RepositoryFormViewModel(new DiskIOWrapper())
            {
                RepositoryPath = "bla",
                CoverageCommand = "bla",
                SourceExtensions = "bla",
            };

            vm.ValidationContext.Text
              .Any(s => s.Contains("CoverageOutputLocation should not be empty"))
              .Should()
              .BeTrue();
        }

        [Fact]
        public void SourceExtensionsEmptyFieldValidation()
        {
            var vm = new RepositoryFormViewModel(new DiskIOWrapper())
            {
                RepositoryPath = "bla",
                CoverageCommand = "bla",
                CoverageOutputLocation = "bla",
            };

            vm.ValidationContext.Text
              .Any(s => s.Contains("SourceExtensions should not be empty"))
              .Should()
              .BeTrue();
        }
    }
}
