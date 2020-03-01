using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Hestia.Model.Wrappers
{
    public class GitCommands : IGitCommands
    {
        private const string AuthorPattern = "\\s*Author:\\s*(.*)";
        private const string ShortLogAuthorPattern = "\\s*\\d+\\s*(.*)";
        private const string CommitHeaderPattern = "^commit\\s+(.*)";
        private readonly ICommandLineExecutor _commandLineExecutor;

        public GitCommands(ICommandLineExecutor commandLineExecutor)
            => _commandLineExecutor = commandLineExecutor;

        public int NumberOfChangesForFile(string filePath) =>
            Exec(OnelineFileHistory(filePath), Path.GetDirectoryName(filePath) ?? string.Empty)
                .Length;

        public int NumberOfChangesForLine(string filePath, int lineNumber) =>
            ParseLineHistoryForNumberOfChanges(Exec(LineHistoryCommand(filePath, lineNumber),
                                                    Path.GetDirectoryName(filePath) ?? string.Empty));

        public IEnumerable<int> NumberOfChangesForEachLine(string filePath, int lineCount) =>
            Enumerable.Range(1, lineCount)
                      .Select(lineNumber => NumberOfChangesForLine(filePath, lineNumber));

        public int NumberOfDifferentAuthorsForFile(string filePath) =>
            ParseShortLogForUniqueAuthors(Exec(AuthorsForFileCommand(filePath), Path.GetDirectoryName(filePath) ?? string.Empty));

        public int NumberOfDifferentAuthorForLine(string filePath, int lineNumber) =>
            ParseNumberOfUniqueAuthorsFromGitHistory(Exec(LineHistoryCommand(filePath,
                                                                             lineNumber),
                                                          Path.GetDirectoryName(filePath) ?? string.Empty));

        public IEnumerable<(int lineNumber, int numberOfAuthors, int numberOfCommits)>
            NumberOfDifferentAuthorsAndChangesForLine(
                string filePath,
                int lineCount) =>
            Enumerable.Range(1, lineCount)
                      .Select(line =>
                                  (line, Exec(LineHistoryCommand(filePath, line), Path.GetDirectoryName(filePath) ?? string.Empty)))
                      .Select(tuple => (tuple.line,
                                        ParseLineHistoryForNumberOfChanges(tuple.Item2),
                                        ParseNumberOfUniqueAuthorsFromGitHistory(tuple.Item2)));

        private string OnelineFileHistory(string filepath) =>
            $"log --pretty=oneline {filepath}";

        private string LineHistoryCommand(string filepath, int lineNumber) =>
            $"log -L {lineNumber},{lineNumber}:\"{filepath}\"";

        private string AuthorsForFileCommand(string filepath) =>
            $"shortlog -c -s {filepath}";

        private int ParseLineHistoryForNumberOfChanges(string[] commandOutput) =>
            commandOutput.Count(line => Regex.IsMatch(line, CommitHeaderPattern));

        private int ParseShortLogForUniqueAuthors(string[] commandOutput) =>
            commandOutput.Select(line => Regex.Match(line, ShortLogAuthorPattern)
                                              .Captures)
                         .Where(capture => capture.Count == 1)
                         .Select(capture => capture[0]
                                     .Value)
                         .Distinct()
                         .Count();

        private int ParseNumberOfUniqueAuthorsFromGitHistory(string[] commandOutput) =>
            commandOutput.Select(line => Regex.Match(line, AuthorPattern)
                                              .Captures.FirstOrDefault())
                         .Where(capture => capture != null && !string.IsNullOrWhiteSpace(capture.Value))
                         .Select(capture => capture.Value)
                         .Distinct()
                         .Count();

        private string[] Exec(string command, string workingDirectory) =>
            _commandLineExecutor.Execute("git", command, workingDirectory);
    }
}
