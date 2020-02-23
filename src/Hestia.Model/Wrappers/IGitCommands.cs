using System.Collections.Generic;

namespace Hestia.Model.Wrappers
{
    public interface IGitCommands
    {
        int NumberOfChangesForFile(string filePath);

        IEnumerable<int> NumberOfChangesForEachLine(string filePath, int lineCount);

        int NumberOfDifferentAuthorsForFile(string filepath);

        int NumberOfDifferentAuthorForLine(string filePath, int lineNumber);

        IEnumerable<(int numberOfAuthors, int numberOfCommits)> NumberOfDifferentAuthorsAndChangesForLine(
            string filePath,
            int lineCount);
    }
}
