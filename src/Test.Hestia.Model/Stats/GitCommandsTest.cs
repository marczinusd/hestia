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
            var fileName = "bla.js";
            var gitCommand = $"git log --pretty=oneline {fileName}";
            executorMock.Setup(mock => mock.Execute(It.Is<string>(command => command == gitCommand)))
                        .Returns(gitLogOutput.Split(Environment.NewLine));
            var gitCommands = new GitCommands(executorMock.Object);

            var result = gitCommands.NumberOfChangesForFile(fileName);

            executorMock.Verify(mock => mock.Execute(gitCommand), Times.Once);
            result.Should()
                  .Be(4);
        }

        [Fact]
        public void SingleLineHistoryTest()
        {
            var lineHistory = Helpers.LoadResource(Resources.Paths.GitLineLogOutput, typeof(GitCommandsTest).Assembly);
            var executorMock = new Mock<ICommandLineExecutor>();
            var fileName = "bla.js";
            var lineNumber = 2;
            var gitCommand = $"git log -L {lineNumber},{lineNumber}:\"{fileName}\"";
            executorMock.Setup(mock => mock.Execute(It.Is<string>(command => command == gitCommand)))
                        .Returns(lineHistory.Split(Environment.NewLine));
            var gitCommands = new GitCommands(executorMock.Object);

            var result = gitCommands.NumberOfChangesForLine(fileName, lineNumber);

            executorMock.Verify(mock => mock.Execute(It.Is<string>(s => s == gitCommand)), Times.Once);
            result.Should()
                  .Be(2);
        }

        [Fact]
        public void AllLinesHistoryTest()
        {
            var lineHistory = Helpers.LoadResource(Resources.Paths.GitLineLogOutput, typeof(GitCommandsTest).Assembly);
            var executorMock = new Mock<ICommandLineExecutor>();
            var fileName = "bla.js";
            var gitCommandFirstLine = $"git log -L 1,1:\"{fileName}\"";
            var gitCommandSecondLine = $"git log -L 2,2:\"{fileName}\"";
            executorMock.Setup(mock => mock.Execute(It.Is<string>(command => command == gitCommandFirstLine)))
                        .Returns(lineHistory.Split(Environment.NewLine));
            executorMock.Setup(mock => mock.Execute(It.Is<string>(command => command == gitCommandSecondLine)))
                        .Returns(lineHistory.Split(Environment.NewLine));
            var gitCommands = new GitCommands(executorMock.Object);

            var result = gitCommands.NumberOfChangesForEachLine(fileName, 2)
                                    .ToArray();

            executorMock.Verify(mock => mock.Execute(It.Is<string>(s => s == gitCommandFirstLine)), Times.Once);
            executorMock.Verify(mock => mock.Execute(It.Is<string>(s => s == gitCommandSecondLine)), Times.Once);
            result.Should()
                  .BeEquivalentTo(new[] { 2, 2 });
        }

        [Fact]
        public void LineAuthorsTest()
        {
            var lineHistory = Helpers.LoadResource(Resources.Paths.GitLineLogOutput, typeof(GitCommandsTest).Assembly);
            var executorMock = new Mock<ICommandLineExecutor>();
            var fileName = "bla.js";
            var lineNumber = 2;
            var gitCommand = $"git log -L {lineNumber},{lineNumber}:\"{fileName}\"";
            executorMock.Setup(mock => mock.Execute(It.Is<string>(command => command == gitCommand)))
                        .Returns(lineHistory.Split(Environment.NewLine));
            var gitCommands = new GitCommands(executorMock.Object);

            var result = gitCommands.NumberOfDifferentAuthorForLine(fileName, lineNumber);

            executorMock.Verify(mock => mock.Execute(It.Is<string>(s => s == gitCommand)), Times.Once);
            result.Should()
                  .Be(2);
        }

        [Fact]
        public void FileAuthorsTest()
        {
            var fileAuthors = Helpers.LoadResource(Resources.Paths.GitShortlogOutput, typeof(GitCommandsTest).Assembly);
            var executorMock = new Mock<ICommandLineExecutor>();
            var fileName = "bla.js";
            var gitCommand = $"git shortlog -c -s {fileName}";
            executorMock.Setup(mock => mock.Execute(It.Is<string>(command => command == gitCommand)))
                        .Returns(fileAuthors.Split(Environment.NewLine));
            var gitCommands = new GitCommands(executorMock.Object);

            var result = gitCommands.NumberOfDifferentAuthorsForFile(fileName);

            executorMock.Verify(mock => mock.Execute(gitCommand), Times.Once);
            result.Should()
                  .Be(2);
        }
    }
}
