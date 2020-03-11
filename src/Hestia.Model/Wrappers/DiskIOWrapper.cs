﻿using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Hestia.Model.Stats;
using LanguageExt;

namespace Hestia.Model.Wrappers
{
    [ExcludeFromCodeCoverage]
    public class DiskIOWrapper : IDiskIOWrapper
    {
        public SourceLine[] ReadAllLinesFromFileAsSourceModel(string filePath)
        {
            return ReadAllLinesFromFile(filePath)
                   .Select((line, i) => new SourceLine(i + 1,
                                                       line,
                                                       Option<LineCoverageStats>.None,
                                                       Option<LineGitStats>.None))
                   .ToArray();
        }

        public IEnumerable<string> ReadAllLinesFromFile(string filePath) => System.IO.File.ReadAllLines(filePath);

        public IEnumerable<string> EnumerateAllDirectoriesForPath(string path) =>
            Directory.GetDirectories(path);

        public IEnumerable<string> EnumerateAllFilesForPath(string path) =>
            Directory.GetFiles(path);

        public IEnumerable<string> EnumerateAllFilesForPathRecursively(string path) =>
            Directory.EnumerateFiles(path, ".*", SearchOption.AllDirectories);

        public string ReadFileContent(string path) => System.IO.File.ReadAllText(path);
    }
}
