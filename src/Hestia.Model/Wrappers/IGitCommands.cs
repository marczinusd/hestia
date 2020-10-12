using System;
using System.Collections.Generic;

namespace Hestia.Model.Wrappers
{
    public interface IGitCommands
    {
        int NumberOfChangesForFile(string filePath);

        IEnumerable<int> NumberOfChangesForEachLine(string filePath, int lineCount);

        int NumberOfDifferentAuthorsForFile(string filePath);

        int NumberOfDifferentAuthorForLine(string filePath, int lineNumber);

        DateTime DateOfLatestCommitOnBranch(string repoPath);

        IEnumerable<(int lineNumber, int numberOfAuthors, int numberOfCommits)>
            NumberOfDifferentAuthorsAndChangesForLine(
                string filePath,
                int lineCount);

        int NumberOfCommitsOnCurrentBranch(string repoPath);

        /// <summary>
        ///     Checks out the nth commit (where 1 is the initial commit) of a git repository.
        /// </summary>
        /// <param name="repoPath">Path to the repo.</param>
        /// <param name="commitNumber">Number of the commit.</param>
        /// <returns>Hash of the commit that was checked out.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the commit number provided was invalid.</exception>
        string CheckoutNthCommitOnBranch(string repoPath, int commitNumber);

        string GetHashForNthCommit(string repoPath, int commitNumber);

        void Checkout(string hash, string repoPath);
    }
}
