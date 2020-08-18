﻿using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Hestia.Model.Interfaces;
using LanguageExt;

namespace Hestia.Model.Wrappers
{
    [ExcludeFromCodeCoverage]
    public class DiskIOWrapper : IDiskIOWrapper
    {
        public ISourceLine[] ReadAllLinesFromFileAsSourceModel(string filePath) =>
            ReadAllLinesFromFile(filePath)
                .Select((line, i) => new SourceLine(i + 1,
                                                    line,
                                                    Option<ILineCoverageStats>.None,
                                                    Option<ILineGitStats>.None) as ISourceLine)
                .ToArray();

        public IEnumerable<string> ReadAllLinesFromFile(string filePath) => System.IO.File.ReadAllLines(filePath);

        public IEnumerable<string> EnumerateAllFilesForPathRecursively(string path) =>
            Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories);

        public string ReadFileContent(string path) => System.IO.File.ReadAllText(path);

        public bool FileExists(string path) => System.IO.File.Exists(path);

        public bool DirectoryExists(string path) => Directory.Exists(path);
    }
}
