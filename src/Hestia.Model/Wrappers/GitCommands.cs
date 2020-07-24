using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using LanguageExt;

namespace Hestia.Model.Wrappers
{
    public class GitCommands : IGitCommands
    {
        private const string AuthorPattern = "\\s*Author:\\s*(.*)";
        private const string ShortLogAuthorPattern = "\\s*\\d+\\s*(.*)";
        private const string CommitHeaderPattern = "^commit\\s+(.*)";
        private const string GitDateFormat = "ddd MMM d HH:mm:ss yyyy K";
        private const string LatestCommitDateCommand = "log -1 --format=%cd";
        private const string CommitCountOnCurrentBranchCommand = "rev-list --count HEAD";
        private readonly ICommandLineExecutor _commandLineExecutor;

        public GitCommands(ICommandLineExecutor commandLineExecutor)
            => _commandLineExecutor = commandLineExecutor;

        public int NumberOfChangesForFile(string filePath)
        {
            var result = Exec(OnelineFileHistory(filePath), Path.GetDirectoryName(filePath) ?? string.Empty);
            return result.Length;
        }

        public int NumberOfChangesForLine(string filePath, int lineNumber) =>
            ParseLineHistoryForNumberOfChanges(Exec(LineHistoryCommand(filePath, lineNumber),
                                                    Path.GetDirectoryName(filePath) ?? string.Empty));

        public IEnumerable<int> NumberOfChangesForEachLine(string filePath, int lineCount) =>
            Enumerable.Range(1, lineCount)
                      .Select(lineNumber => NumberOfChangesForLine(filePath, lineNumber));

        public int NumberOfDifferentAuthorsForFile(string filepath) =>
            ParseShortLogForUniqueAuthors(Exec(AuthorsForFileCommand(filepath),
                                               Path.GetDirectoryName(filepath) ?? string.Empty));

        public int NumberOfDifferentAuthorForLine(string filePath, int lineNumber) =>
            ParseNumberOfUniqueAuthorsFromGitHistory(Exec(LineHistoryCommand(filePath,
                                                                             lineNumber),
                                                          Path.GetDirectoryName(filePath) ?? string.Empty));

        public DateTime DateOfLatestCommitOnBranch(string repoPath) =>
            DateTime.ParseExact(Exec(LatestCommitDateCommand, repoPath)
                                    .First(),
                                GitDateFormat,
                                CultureInfo.InvariantCulture,
                                DateTimeStyles.None);

        public int NumberOfCommitsOnCurrentBranch(string repoPath) =>
            int.Parse(Exec(CommitCountOnCurrentBranchCommand, repoPath)
                      .First()
                      .Trim());

        /// <summary>
        /// Checks out the nth commit (where 1 is the initial commit) of a git repository.
        /// </summary>
        /// <param name="repoPath">Path to the repo.</param>
        /// <param name="commitNumber">Number of the commit.</param>
        /// <returns>Hash of the commit that was checked out.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the commit number provided was invalid.</exception>
        public string CheckoutNthCommitOnBranch(string repoPath, int commitNumber)
        {
            var numberOfCommits = NumberOfCommitsOnCurrentBranch(repoPath);
            if (commitNumber > numberOfCommits)
            {
                throw new
                    ArgumentOutOfRangeException($"{nameof(commitNumber)} with {commitNumber} was out of range: this branch only has {numberOfCommits} commits");
            }

            var hash = GetHashForNthCommit(repoPath, commitNumber);
            Exec(CheckoutCommand(hash), repoPath);

            return hash;
        }

        public IEnumerable<(int lineNumber, int numberOfAuthors, int numberOfCommits)>
            NumberOfDifferentAuthorsAndChangesForLine(
                string filePath,
                int lineCount) =>
            Enumerable.Range(1, lineCount)
                      .Select(line =>
                                  (line,
                                   Exec(LineHistoryCommand(filePath, line),
                                        Path.GetDirectoryName(filePath) ?? string.Empty)))
                      .Select(tuple => (tuple.line,
                                        ParseLineHistoryForNumberOfChanges(tuple.Item2),
                                        ParseNumberOfUniqueAuthorsFromGitHistory(tuple.Item2)));

        public string GetHashForNthCommit(string repoPath, int commitNumber)
        {
            var numberOfCommitsOnBranch = NumberOfCommitsOnCurrentBranch(repoPath);

            return Exec(HashForNthCommitCommand(numberOfCommitsOnBranch - commitNumber + 1), repoPath)
                   .Single(l => Regex.IsMatch(l, CommitHeaderPattern))
                   .Apply(x => Regex.Match(x, CommitHeaderPattern)
                                    .Groups[1]
                                    .Value);
        }

        public void Checkout(string hash, string repoPath) =>
            Exec(CheckoutCommand(hash), repoPath);

        private static string OnelineFileHistory(string filepath) =>
            $"log --pretty=oneline {filepath}";

        private static string LineHistoryCommand(string filepath, int lineNumber) =>
            $"log -L {lineNumber},{lineNumber}:\"{filepath}\"";

        private static string AuthorsForFileCommand(string filepath) =>
            $"shortlog -c -s {filepath}";

        private static string HashForNthCommitCommand(int commitNumber) =>
            $"log -1 HEAD~{commitNumber}";

        private static string CheckoutCommand(string hash) =>
            $"checkout {hash}";

        private static int ParseLineHistoryForNumberOfChanges(string[] commandOutput) =>
            commandOutput.Count(line => Regex.IsMatch(line, CommitHeaderPattern));

        private static int ParseShortLogForUniqueAuthors(string[] commandOutput) =>
            commandOutput.Select(line => Regex.Match(line, ShortLogAuthorPattern)
                                              .Captures)
                         .Where(capture => capture.Count == 1)
                         .Select(capture => capture[0]
                                     .Value)
                         .Distinct()
                         .Count();

        private static int ParseNumberOfUniqueAuthorsFromGitHistory(string[] commandOutput) =>
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
