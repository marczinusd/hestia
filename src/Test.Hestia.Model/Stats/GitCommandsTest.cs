using System;
using System.Linq;
using FluentAssertions;
using Hestia.Model.Wrappers;
using Moq;
using Test.Hestia.Model.Utils;
using Xunit;

namespace Test.Hestia.Model.Stats
{
    public class GitCommandsTest
    {
        [Fact]
        public void FileHistoryTest()
        {
            var gitLogOutput =
                Helpers.LoadResource(Resources.Paths.GitPrettyLogOutput, typeof(GitCommandsTest).Assembly);
            var executorMock = new Mock<ICommandLineExecutor>();
            var fileName = "dir/bla.js";
            var gitCommand = $"log --pretty=oneline {fileName}";
            executorMock.Setup(mock => mock.Execute("git", It.Is<string>(command => command == gitCommand), "dir"))
                        .Returns(gitLogOutput.Split(Environment.NewLine));
            var gitCommands = new GitCommands(executorMock.Object);

            var result = gitCommands.NumberOfChangesForFile(fileName);

            executorMock.Verify(mock => mock.Execute("git", gitCommand, "dir"), Times.Once);
            result.Should()
                  .Be(4);
        }

        [Fact]
        public void SingleLineHistoryTest()
        {
            var lineHistory = Helpers.LoadResource(Resources.Paths.GitLineLogOutput, typeof(GitCommandsTest).Assembly);
            var executorMock = new Mock<ICommandLineExecutor>();
            var fileName = "dir/bla.js";
            var lineNumber = 2;
            var gitCommand = $"log -L {lineNumber},{lineNumber}:\"{fileName}\"";
            executorMock.Setup(mock => mock.Execute("git", It.Is<string>(command => command == gitCommand), "dir"))
                        .Returns(lineHistory.Split(Environment.NewLine));
            var gitCommands = new GitCommands(executorMock.Object);

            var result = gitCommands.NumberOfChangesForLine(fileName, lineNumber);

            executorMock.Verify(mock => mock.Execute("git", It.Is<string>(s => s == gitCommand), "dir"), Times.Once);
            result.Should()
                  .Be(2);
        }

        [Fact]
        public void AllLinesHistoryTest()
        {
            var lineHistory = Helpers.LoadResource(Resources.Paths.GitLineLogOutput, typeof(GitCommandsTest).Assembly);
            var executorMock = new Mock<ICommandLineExecutor>();
            var fileName = "dir/bla.js";
            var gitCommandFirstLine = $"log -L 1,1:\"{fileName}\"";
            var gitCommandSecondLine = $"log -L 2,2:\"{fileName}\"";
            executorMock.Setup(mock => mock.Execute("git", It.Is<string>(command => command == gitCommandFirstLine), "dir"))
                        .Returns(lineHistory.Split(Environment.NewLine));
            executorMock.Setup(mock => mock.Execute("git", It.Is<string>(command => command == gitCommandSecondLine), "dir"))
                        .Returns(lineHistory.Split(Environment.NewLine));
            var gitCommands = new GitCommands(executorMock.Object);

            var result = gitCommands.NumberOfChangesForEachLine(fileName, 2)
                                    .ToArray();

            executorMock.Verify(mock => mock.Execute("git", It.Is<string>(s => s == gitCommandFirstLine), "dir"), Times.Once);
            executorMock.Verify(mock => mock.Execute("git", It.Is<string>(s => s == gitCommandSecondLine), "dir"), Times.Once);
            result.Should()
                  .BeEquivalentTo(new[] { 2, 2 });
        }

        [Fact]
        public void LineAuthorsTest()
        {
            var lineHistory = Helpers.LoadResource(Resources.Paths.GitLineLogOutput, typeof(GitCommandsTest).Assembly);
            var executorMock = new Mock<ICommandLineExecutor>();
            var fileName = "dir/bla.js";
            var lineNumber = 2;
            var gitCommand = $"log -L {lineNumber},{lineNumber}:\"{fileName}\"";
            executorMock.Setup(mock => mock.Execute("git", It.Is<string>(command => command == gitCommand), "dir"))
                        .Returns(lineHistory.Split(Environment.NewLine));
            var gitCommands = new GitCommands(executorMock.Object);

            var result = gitCommands.NumberOfDifferentAuthorForLine(fileName, lineNumber);

            executorMock.Verify(mock => mock.Execute("git", It.Is<string>(s => s == gitCommand), "dir"), Times.Once);
            result.Should()
                  .Be(2);
        }

        [Fact]
        public void FileAuthorsTest()
        {
            var fileAuthors = Helpers.LoadResource(Resources.Paths.GitShortlogOutput, typeof(GitCommandsTest).Assembly);
            var executorMock = new Mock<ICommandLineExecutor>();
            var fileName = "dir/bla.js";
            var gitCommand = $"shortlog -c -s {fileName}";
            executorMock.Setup(mock => mock.Execute("git", It.Is<string>(command => command == gitCommand), "dir"))
                        .Returns(fileAuthors.Split(Environment.NewLine));
            var gitCommands = new GitCommands(executorMock.Object);

            var result = gitCommands.NumberOfDifferentAuthorsForFile(fileName);

            executorMock.Verify(mock => mock.Execute("git", gitCommand, "dir"), Times.Once);
            result.Should()
                  .Be(2);
        }
    }
}
