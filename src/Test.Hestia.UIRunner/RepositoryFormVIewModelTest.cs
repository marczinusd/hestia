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

        public static TheoryData<string> EmptyInputData => new TheoryData<string> { string.Empty, null, "     " };

        [Fact]
        public void RepositoryPathShouldPointToDirectoryThatExists()
        {
            var ioMock = new Mock<IDiskIOWrapper>();
            ioMock.Setup(mock => mock.DirectoryExists(RepoPath))
                  .Returns(false);

            var vm = new FormViewModel(ioMock.Object) { RepositoryPath = RepoPath };

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

            var vm = new FormViewModel(ioMock.Object) { RepositoryPath = RepoPath };

            vm.ValidationContext.Text
              .First()
              .Should()
              .Contain("Directory is not a git repository");
        }

        [Theory]
        [MemberData(nameof(EmptyInputData))]
        public void RepositoryPathEmptyFieldValidation(string input)
        {
            var vm = new FormViewModel(new DiskIOWrapper())
            {
                CoverageCommand = "bla",
                SourceExtensions = "bla",
                CoverageOutputLocation = "bla",
                RepositoryPath = input,
            };

            vm.ValidationContext.Text
              .Any(s => s.Contains("RepositoryPath should not be empty"))
              .Should()
              .BeTrue();
        }

        [Theory]
        [MemberData(nameof(EmptyInputData))]
        public void CoverageCommandEmptyFieldValidation(string input)
        {
            var vm = new FormViewModel(new DiskIOWrapper())
            {
                RepositoryPath = "bla",
                SourceExtensions = "bla",
                CoverageOutputLocation = "bla",
                CoverageCommand = input,
            };

            vm.ValidationContext.Text
              .Any(s => s.Contains("CoverageCommand should not be empty"))
              .Should()
              .BeTrue();
        }

        [Theory]
        [MemberData(nameof(EmptyInputData))]
        public void CoverageOutputLocationEmptyFieldValidation(string input)
        {
            var vm = new FormViewModel(new DiskIOWrapper())
            {
                RepositoryPath = "bla",
                CoverageCommand = "bla",
                SourceExtensions = "bla",
                CoverageOutputLocation = input,
            };

            vm.ValidationContext.Text
              .Any(s => s.Contains("CoverageOutputLocation should not be empty"))
              .Should()
              .BeTrue();
        }

        [Theory]
        [MemberData(nameof(EmptyInputData))]
        public void SourceExtensionsEmptyFieldValidation(string input)
        {
            var vm = new FormViewModel(new DiskIOWrapper())
            {
                RepositoryPath = "bla",
                CoverageCommand = "bla",
                CoverageOutputLocation = "bla",
                SourceExtensions = input,
            };

            vm.ValidationContext.Text
              .Any(s => s.Contains("SourceExtensions should not be empty"))
              .Should()
              .BeTrue();
        }
    }
}
