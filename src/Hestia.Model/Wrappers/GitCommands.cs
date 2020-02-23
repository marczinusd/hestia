using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Hestia.Model.Wrappers
{
    public class GitCommands : IGitCommands
    {
        private const string AuthorPattern = "\\s*Author:\\s*(.*)";
        private const string ShortLogAuthorPattern = "\\s*(\\d+)\\s*(.*)$";
        private const string CommitHeaderPattern = "^commit\\s+(.*)";
        private readonly ICommandLineExecutor _commandLineExecutor;

        public GitCommands(ICommandLineExecutor commandLineExecutor)
            => _commandLineExecutor = commandLineExecutor;

        public int NumberOfChangesForFile(string filePath) =>
            _commandLineExecutor
                .Execute(OnelineFileHistory(filePath))
                .Length;

        public IEnumerable<int> NumberOfChangesForEachLine(string filePath, int lineCount) =>
            Enumerable.Range(1, lineCount)
                      .Select(lineNumber =>
                                  ParseLineHistoryForNumberOfChanges(_commandLineExecutor
                                                                         .Execute(LineHistoryCommand(filePath,
                                                                                                     lineNumber))));

        public int NumberOfDifferentAuthorsForFile(string filepath) =>
            ParseShortLogForUniqueAuthors(Exec(AuthorsForFileCommand(filepath)));

        public int NumberOfDifferentAuthorForLine(string filePath, int lineNumber) =>
            ParseNumberOfUniqueAuthorsFromGitHistory(Exec(LineHistoryCommand(filePath,
                                                                             lineNumber)));

        public IEnumerable<(int numberOfAuthors, int numberOfCommits)> NumberOfDifferentAuthorsAndChangesForLine(
            string filePath,
            int lineCount) =>
            Enumerable.Range(1, lineCount)
                      .Select(line => Exec(LineHistoryCommand(filePath, line)))
                      .Select(output => (ParseLineHistoryForNumberOfChanges(output),
                                         ParseNumberOfUniqueAuthorsFromGitHistory(output)));

        private string OnelineFileHistory(string filepath) =>
            $"git log --pretty=oneline {filepath}";

        private string LineHistoryCommand(string filepath, int lineNumber) =>
            $"git log -L {lineNumber},{lineNumber} {filepath}";

        private string AuthorsForFileCommand(string filepath) =>
            $"git shortlog -c -s {filepath}";

        private int ParseLineHistoryForNumberOfChanges(string[] commandOutput) =>
            commandOutput.Select(line => Regex.Match(line, CommitHeaderPattern))
                         .Count();

        private int ParseShortLogForUniqueAuthors(string[] commandOutput) =>
            commandOutput.Select(line => Regex.Match(line, ShortLogAuthorPattern)
                                              .Captures)
                         .Where(capture => capture.Count == 2)
                         .Select(capture => capture[1])
                         .Distinct()
                         .Count();

        private int ParseNumberOfUniqueAuthorsFromGitHistory(string[] commandOutput) =>
            commandOutput.Select(line => Regex.Match(line, AuthorPattern)
                                              .Captures.FirstOrDefault())
                         .Where(capture => capture != null && !string.IsNullOrWhiteSpace(capture.Value))
                         .Select(capture => capture.Value)
                         .Distinct()
                         .Count();

        private string[] Exec(string command) => _commandLineExecutor.Execute(command);
    }
}
