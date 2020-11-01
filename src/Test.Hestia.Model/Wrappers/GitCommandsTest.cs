using System;
using System.Linq;
using FluentAssertions;
using Hestia.Model.Wrappers;
using Moq;
using Test.Hestia.Model.Resources;
using Test.Hestia.Utils;
using Xunit;

namespace Test.Hestia.Model.Wrappers
{
    public class GitCommandsTest
    {
        [Fact]
        public void FileHistoryTest()
        {
            var gitLogOutput =
                Helpers.LoadResource(Paths.GitPrettyLogOutput, typeof(GitCommandsTest).Assembly);
            var executorMock = new Mock<ICommandLineExecutor>();
            var fileName = "dir/bla.js";
            var gitCommand = $"log --pretty=oneline {fileName}";
            executorMock.Setup(mock => mock.Execute("git", It.Is<string>(command => command == gitCommand), "dir"))
                        .Returns(gitLogOutput.Split('\n'));
            var gitCommands = new GitCommands(executorMock.Object);

            var result = gitCommands.NumberOfChangesForFile(fileName);

            executorMock.Verify(mock => mock.Execute("git", gitCommand, "dir"), Times.Once);
            result.Should()
                  .Be(4);
        }

        [Fact]
        public void NumberOfChangesForFileThrowsArgumentExceptionIfFilePathIsInvalid()
        {
            var gitCommands = new GitCommands(Mock.Of<ICommandLineExecutor>());

            Action act = () => gitCommands.NumberOfChangesForFile(string.Empty);

            act.Should()
               .Throw<ArgumentException>();
        }

        [Fact]
        public void NumberOfDifferentAuthorForLineThrowsArgumentExceptionIfFilePathIsInvalid()
        {
            var gitCommands = new GitCommands(Mock.Of<ICommandLineExecutor>());

            Action act = () => gitCommands.NumberOfDifferentAuthorForLine(string.Empty, 1);

            act.Should()
               .Throw<ArgumentException>();
        }

        [Fact]
        public void NumberOfDifferentAuthorsForFileThrowsArgumentExceptionIfFilePathIsInvalid()
        {
            var gitCommands = new GitCommands(Mock.Of<ICommandLineExecutor>());

            Action act = () => gitCommands.NumberOfDifferentAuthorsForFile(string.Empty);

            act.Should()
               .Throw<ArgumentException>();
        }

        [Fact]
        public void NumberOfDifferentAuthorsAndChangesForLineThrowsArgumentExceptionIfFilePathIsInvalid()
        {
            var gitCommands = new GitCommands(Mock.Of<ICommandLineExecutor>());

            Action act = () => gitCommands.NumberOfDifferentAuthorsAndChangesForLine(string.Empty, 3);

            act.Should()
               .Throw<ArgumentException>();
        }

        [Fact]
        public void NumberOfChangesForLineThrowsArgumentExceptionIfFilePathIsInvalid()
        {
            var gitCommands = new GitCommands(Mock.Of<ICommandLineExecutor>());

            Action act = () => gitCommands.NumberOfChangesForLine(string.Empty, 2);

            act.Should()
               .Throw<ArgumentException>();
        }

        [Fact]
        public void SingleLineHistoryTest()
        {
            var lineHistory = Helpers.LoadResource(Paths.GitLineLogOutput, typeof(GitCommandsTest).Assembly);
            var executorMock = new Mock<ICommandLineExecutor>();
            var fileName = "dir/bla.js";
            var lineNumber = 2;
            var gitCommand = $"log --pretty='format:commit:(%h) author:(%an)' -L {lineNumber},{lineNumber}:\"{fileName}\" --no-patch";
            executorMock.Setup(mock => mock.Execute("git", It.Is<string>(command => command == gitCommand), "dir"))
                        .Returns(lineHistory.Split('\n'));
            var gitCommands = new GitCommands(executorMock.Object);

            var result = gitCommands.NumberOfChangesForLine(fileName, lineNumber);

            executorMock.Verify(mock => mock.Execute("git", It.Is<string>(s => s == gitCommand), "dir"), Times.Once);
            result.Should()
                  .Be(9);
        }

        [Fact]
        public void AllLinesHistoryTest()
        {
            var lineHistory = Helpers.LoadResource(Paths.GitLineLogOutput, typeof(GitCommandsTest).Assembly);
            var executorMock = new Mock<ICommandLineExecutor>();
            var fileName = "dir/bla.js";
            var gitCommandFirstLine = $"log --pretty='format:commit:(%h) author:(%an)' -L 1,1:\"{fileName}\" --no-patch";
            var gitCommandSecondLine = $"log --pretty='format:commit:(%h) author:(%an)' -L 2,2:\"{fileName}\" --no-patch";
            executorMock
                .Setup(mock => mock.Execute("git", It.Is<string>(command => command == gitCommandFirstLine), "dir"))
                .Returns(lineHistory.Split('\n'));
            executorMock
                .Setup(mock => mock.Execute("git", It.Is<string>(command => command == gitCommandSecondLine), "dir"))
                .Returns(lineHistory.Split('\n'));
            var gitCommands = new GitCommands(executorMock.Object);

            var result = gitCommands.NumberOfChangesForEachLine(fileName, 2)
                                    .ToArray();

            executorMock.Verify(mock => mock.Execute("git", It.Is<string>(s => s == gitCommandFirstLine), "dir"),
                                Times.Once);
            executorMock.Verify(mock => mock.Execute("git", It.Is<string>(s => s == gitCommandSecondLine), "dir"),
                                Times.Once);
            result.Should()
                  .BeEquivalentTo(new[] { 9, 9 });
        }

        [Fact]
        public void LineAuthorsTest()
        {
            var lineHistory = Helpers.LoadResource(Paths.GitLineLogOutput, typeof(GitCommandsTest).Assembly);
            var executorMock = new Mock<ICommandLineExecutor>();
            var fileName = "dir/bla.js";
            var lineNumber = 2;
            var gitCommand = $"log --pretty='format:commit:(%h) author:(%an)' -L {lineNumber},{lineNumber}:\"{fileName}\" --no-patch";
            executorMock.Setup(mock => mock.Execute("git", It.Is<string>(command => command == gitCommand), "dir"))
                        .Returns(lineHistory.Split('\n'));
            var gitCommands = new GitCommands(executorMock.Object);

            var result = gitCommands.NumberOfDifferentAuthorForLine(fileName, lineNumber);

            executorMock.Verify(mock => mock.Execute("git", It.Is<string>(s => s == gitCommand), "dir"), Times.Once);
            result.Should()
                  .Be(4);
        }

        [Fact]
        public void FileAuthorsTest()
        {
            var fileAuthors = Helpers.LoadResource(Paths.GitShortlogOutput, typeof(GitCommandsTest).Assembly);
            var executorMock = new Mock<ICommandLineExecutor>();
            var fileName = "dir/bla.js";
            var gitCommand = $"shortlog -c -s {fileName}";
            executorMock.Setup(mock => mock.Execute("git", It.Is<string>(command => command == gitCommand), "dir"))
                        .Returns(fileAuthors.Split('\n'));
            var gitCommands = new GitCommands(executorMock.Object);

            var result = gitCommands.NumberOfDifferentAuthorsForFile(fileName);

            executorMock.Verify(mock => mock.Execute("git", gitCommand, "dir"), Times.Once);
            result.Should()
                  .Be(2);
        }

        [Fact]
        public void LatestCommitDateTest()
        {
            var executorMock = new Mock<ICommandLineExecutor>();
            const string repoPath = "dir";
            const string gitCommand = "log -1 --format=%cd";
            executorMock.Setup(mock => mock.Execute("git", It.Is<string>(command => command == gitCommand), repoPath))
                        .Returns(new[] { "Thu Mar 12 23:38:51 2020 +0100" });
            var gitCommands = new GitCommands(executorMock.Object);

            var result = gitCommands.DateOfLatestCommitOnBranch(repoPath);

            executorMock.Verify(mock => mock.Execute("git", gitCommand, repoPath), Times.Once);
            result.Date.Should()
                  .BeSameDateAs(new DateTime(2020, 3, 12));
        }

        [Fact]
        public void NumberOfCommitsOnCurrentBranchTest()
        {
            var executorMock = new Mock<ICommandLineExecutor>();
            const string repoPath = "dir";
            const string gitCommand = "rev-list --count HEAD";
            executorMock.Setup(mock => mock.Execute("git", It.Is<string>(command => command == gitCommand), repoPath))
                        .Returns(new[] { "123" });
            var gitCommands = new GitCommands(executorMock.Object);

            var result = gitCommands.NumberOfCommitsOnCurrentBranch(repoPath);

            executorMock.Verify(mock => mock.Execute("git", gitCommand, repoPath), Times.Once);
            result.Should()
                  .Be(123);
        }

        [Fact]
        public void HashForNthCommit()
        {
            var output = Helpers.LoadResource(Paths.GitSingleCommitOutput, typeof(GitCommandsTest).Assembly);
            var executorMock = new Mock<ICommandLineExecutor>();
            const string repoPath = "dir";
            const int nthCommit = 5;
            const int allCommits = 123;
            const string commitCountCommand = "rev-list --count HEAD";
            var nthCommitHashCommand = $"log -1 HEAD~{allCommits - nthCommit + 1}";
            executorMock
                .Setup(mock => mock.Execute("git", It.Is<string>(command => command == commitCountCommand), repoPath))
                .Returns(new[] { allCommits.ToString() });
            executorMock
                .Setup(mock => mock.Execute("git", It.Is<string>(command => command == nthCommitHashCommand), repoPath))
                .Returns(output.Split('\n'));
            var gitCommands = new GitCommands(executorMock.Object);

            var result = gitCommands.GetHashForNthCommit(repoPath, nthCommit);

            executorMock.Verify(mock => mock.Execute("git", commitCountCommand, repoPath), Times.Once);
            executorMock.Verify(mock => mock.Execute("git", nthCommitHashCommand, repoPath), Times.Once);
            result.Should()
                  .Be("abb3cc3d7e405c39eae91b22c41d1281b9075cd4");
        }

        [Fact]
        public void HashForLatestCommit()
        {
            var executorMock = new Mock<ICommandLineExecutor>();
            const string repoPath = "dir";
            const string command = "rev-parse HEAD";
            executorMock
                .Setup(mock => mock.Execute("git", It.Is<string>(c => c == command), repoPath))
                .Returns(new[] { "23d81a11e8deb30d2f004cfc5ed2d7430581e70a" });
            var gitCommands = new GitCommands(executorMock.Object);

            var result = gitCommands.GetHashForLatestCommit(repoPath);

            result.Should()
                  .Be("23d81a11e8deb30d2f004cfc5ed2d7430581e70a");
        }

        [Fact]
        public void CheckoutNthCommitOnBranch()
        {
            var output = Helpers.LoadResource(Paths.GitSingleCommitOutput, typeof(GitCommandsTest).Assembly);
            var executorMock = new Mock<ICommandLineExecutor>();
            const string repoPath = "dir";
            const string commitCountCommand = "rev-list --count HEAD";
            const int nthCommit = 5;
            const int allCommits = 123;
            var nthCommitHashCommand = $"log -1 HEAD~{allCommits - nthCommit + 1}";
            executorMock
                .Setup(mock => mock.Execute("git", It.Is<string>(command => command == commitCountCommand), repoPath))
                .Returns(new[] { allCommits.ToString() });
            executorMock
                .Setup(mock => mock.Execute("git", It.Is<string>(command => command == nthCommitHashCommand), repoPath))
                .Returns(output.Split('\n'));
            var gitCommands = new GitCommands(executorMock.Object);

            gitCommands.CheckoutNthCommitOnBranch(repoPath, nthCommit);

            executorMock.Verify(mock => mock.Execute("git", commitCountCommand, repoPath), Times.Exactly(2));
            executorMock.Verify(mock => mock.Execute("git", nthCommitHashCommand, repoPath), Times.Once);
            executorMock.Verify(mock => mock.Execute("git",
                                                     "checkout abb3cc3d7e405c39eae91b22c41d1281b9075cd4",
                                                     repoPath),
                                Times.Once);
        }

        [Fact]
        public void GetOrderOfCurrentHeadRelativeToFirstCommitOfBranchTest()
        {
            var output = Helpers.LoadResource(Paths.GitSingleCommitOutput, typeof(GitCommandsTest).Assembly);
            var executorMock = new Mock<ICommandLineExecutor>();
            var gitCommands = new GitCommands(executorMock.Object);
            const int nthCommit = 1;
            const int allCommits = 123;
            executorMock.Setup(mock => mock.Execute("git", "rev-list --count HEAD", "dir"))
                        .Returns(new[] { "123" });
            var nthCommitHashCommand = $"log -1 HEAD~{allCommits - nthCommit + 1}";
            executorMock.Setup(mock => mock.Execute("git", nthCommitHashCommand, "dir"))
                        .Returns(output.Split('\n'));
            executorMock.Setup(mock => mock.Execute("git",
                                                    "rev-list abb3cc3d7e405c39eae91b22c41d1281b9075cd4..HEAD --count",
                                                    "dir"))
                        .Returns(new[] { "42" });

            var result = gitCommands.GetOrderOfCurrentHeadRelativeToFirstCommitOfBranch("dir");

            executorMock.Verify(mock => mock.Execute("git", "rev-list --count HEAD", "dir"), Times.Once);
            executorMock.Verify(mock => mock.Execute("git", nthCommitHashCommand, "dir"), Times.Once);
            executorMock.Verify(mock => mock.Execute("git",
                                                     "rev-list abb3cc3d7e405c39eae91b22c41d1281b9075cd4..HEAD --count",
                                                     "dir"),
                                Times.Once);
            result.Should()
                  .Be(42);
        }

        [Fact]
        public void CheckoutNthCommitOnBranchThrowsExceptionWhenCommitNumberIsGreaterThanAllCommitNumber()
        {
            var executorMock = new Mock<ICommandLineExecutor>();
            const string repoPath = "dir";
            const string commitCountCommand = "rev-list --count HEAD";
            const int nthCommit = 124;
            const int allCommits = 123;
            executorMock
                .Setup(mock => mock.Execute("git", It.Is<string>(command => command == commitCountCommand), repoPath))
                .Returns(new[] { allCommits.ToString() });
            var gitCommands = new GitCommands(executorMock.Object);

            Action act = () => gitCommands.CheckoutNthCommitOnBranch(repoPath, nthCommit);

            act.Should()
               .Throw<ArgumentOutOfRangeException>()
               .WithMessage("*this branch only has*");
        }

        [Fact]
        public void NumberOfDifferentAuthorsAndChangesForLine()
        {
            var executorMock = new Mock<ICommandLineExecutor>();
            var lineHistory = Helpers.LoadResource(Paths.GitLineLogOutput, typeof(GitCommandsTest).Assembly);
            const string repoPath = "dir";
            const string filePath = "dir/file.js";
            const string gitCommand = "log --pretty='format:commit:(%h) author:(%an)' -L {0},{0}:\"{1}\" --no-patch";
            var gitCommands = new GitCommands(executorMock.Object);
            for (var i = 1; i < 4; i++)
            {
                var i1 = i;
                executorMock.Setup(mock => mock.Execute("git", string.Format(gitCommand, i1, filePath), repoPath))
                            .Returns(lineHistory.Split('\n'));
            }

            var result = gitCommands.NumberOfDifferentAuthorsAndChangesForLine(filePath, 3)
                                    .ToList();

            executorMock.Verify(mock => mock.Execute("git", It.IsAny<string>(), repoPath),
                                Times.Exactly(3));
            result.Should()
                  .BeEquivalentTo(new[] { (1, 9, 4), (2, 9, 4), (3, 9, 4) });
        }

        [Fact]
        public void CheckoutTest()
        {
            var executorMock = new Mock<ICommandLineExecutor>();
            const string repoPath = "dir";
            const string gitCommand = "checkout hash";
            var gitCommands = new GitCommands(executorMock.Object);

            gitCommands.Checkout("hash", repoPath);

            executorMock.Verify(mock => mock.Execute("git", gitCommand, repoPath), Times.Once);
        }
    }
}
