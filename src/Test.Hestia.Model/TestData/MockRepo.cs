using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bogus;
using Hestia.Model;
using Hestia.Model.Builders;
using Hestia.Model.Stats;
using Hestia.Model.Wrappers;
using LanguageExt;
using Moq;
using File = Hestia.Model.File;

namespace Test.Hestia.Model.TestData
{
    public static class MockRepo
    {
        private static readonly string DirPath = Path.Join("C:", "temp");

        public static string FirstIncludedFilePath { get; } = Path.Join(DirPath,
                                                                        "src",
                                                                        "model",
                                                                        "File.cs");

        public static int FirstIncludedFileGitStats { get; } = 2;

        public static string SecondIncludedFilePath { get; } = Path.Join(DirPath,
                                                                         "src",
                                                                         "model",
                                                                         "something",
                                                                         "bla2.cs");

        public static int SecondIncludedFileGitStats { get; } = 3;

        public static FileCoverageStats DefaultCoverage { get; } =
            new FileCoverageStats(new FileCoverage(string.Empty, new (int lineNumber, int hitCount)[0]));

        public static Mock<IDiskIOWrapper> CreateDiskIOWrapperMock()
        {
            var ioWrapper = new Mock<IDiskIOWrapper>();
            ioWrapper.Setup(mock => mock.EnumerateAllFilesForPathRecursively(It.IsAny<string>()))
                     .Returns(new[]
                     {
                         Path.Join(DirPath, "bla2.js"), FirstIncludedFilePath, SecondIncludedFilePath,
                         Path.Join(DirPath,
                                   "src",
                                   "server",
                                   "bla2.js"),
                     });
            ioWrapper.Setup(mock => mock.ReadAllLinesFromFile(It.IsAny<string>()))
                     .Returns(new List<string> { "bla", "bla", "bla" });
            ioWrapper.Setup(mock => mock.ReadAllLinesFromFileAsSourceModel(It.IsAny<string>()))
                     .Returns(new[]
                     {
                         new SourceLine(1,
                                        "bla",
                                        Option<LineCoverageStats>.None,
                                        Option<LineGitStats>.None),
                         new SourceLine(2,
                                        "bla",
                                        Option<LineCoverageStats>.None,
                                        Option<LineGitStats>.None),
                         new SourceLine(3,
                                        "bla",
                                        Option<LineCoverageStats>.None,
                                        Option<LineGitStats>.None),
                         new SourceLine(4,
                                        "bla",
                                        Option<LineCoverageStats>.None,
                                        Option<LineGitStats>.None),
                         new SourceLine(5,
                                        "bla",
                                        Option<LineCoverageStats>.None,
                                        Option<LineGitStats>.None),
                     });

            return ioWrapper;
        }

        public static Mock<IGitCommands> CreateGitCommandsMock()
        {
            var mock = new Mock<IGitCommands>();

            mock.Setup(m => m.NumberOfChangesForFile(FirstIncludedFilePath))
                .Returns(FirstIncludedFileGitStats);
            mock.Setup(m => m.NumberOfChangesForFile(SecondIncludedFilePath))
                .Returns(SecondIncludedFileGitStats);

            mock.Setup(m => m.NumberOfDifferentAuthorsForFile(FirstIncludedFilePath))
                .Returns(FirstIncludedFileGitStats);
            mock.Setup(m => m.NumberOfDifferentAuthorsForFile(SecondIncludedFilePath))
                .Returns(SecondIncludedFileGitStats);

            mock.Setup(m => m.NumberOfChangesForEachLine(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(Enumerable.Repeat(5, 100));
            mock.Setup(m => m.NumberOfDifferentAuthorsAndChangesForLine(It.IsAny<string>(), It.IsAny<int>()))
                .Returns<string, int>((s, i) => new[] { (1, 2, 2), (2, 2, 2), (3, 2, 2) });

            return mock;
        }

        public static File CreateFile() =>
            CreateFileFaker()
                .Generate();

        public static IEnumerable<File> CreateFiles(int count) =>
            CreateFileFaker()
                .GenerateLazy(count);

        public static RepositorySnapshot CreateSnapshot(IEnumerable<string> extensions,
                                                        string coveragePath,
                                                        IDiskIOWrapper ioWrapper,
                                                        IPathValidator validator) =>
            new RepositorySnapshotBuilderArguments(1,
                                                   DirPath,
                                                   string.Empty,
                                                   extensions.ToArray(),
                                                   coveragePath,
                                                   Option<string>.None,
                                                   Option<DateTime>.None,
                                                   ioWrapper,
                                                   validator).Build();

        private static Faker<File> CreateFileFaker() =>
            new Faker<File>()
                .CustomInstantiator(f => new File(f.System.FileName(),
                                                  f.System.FileExt(),
                                                  f.System.FilePath(),
                                                  f.Lorem.Lines(10, Environment.NewLine)
                                                   .Split(Environment.NewLine)
                                                   .Select((l, i) => new SourceLine(i + 1,
                                                                                    l,
                                                                                    Option<LineCoverageStats>
                                                                                        .None,
                                                                                    Option<LineGitStats>.None))
                                                   .ToList(),
                                                  new FileGitStats(1, 1),
                                                  new FileCoverageStats(new FileCoverage(string.Empty,
                                                                                         new[] { (1, 2) }))))
                .RuleFor(file => file.Content,
                         f => f.Lorem.Lines(10, Environment.NewLine)
                               .Split(Environment.NewLine)
                               .Select((l, i) => new SourceLine(i + 1,
                                                                l,
                                                                Option<LineCoverageStats>.None,
                                                                Option<LineGitStats>.None))
                               .ToList())
                .RuleFor(file => file.Extension, f => f.System.FileExt())
                .RuleFor(file => file.Filename, f => f.System.FileName())
                .RuleFor(file => file.Path, f => f.System.FilePath())
                .RuleFor(file => file.GitStats, f => new FileGitStats(1, 1))
                .RuleFor(file => file.CoverageStats,
                         f => new FileCoverageStats(new FileCoverage(string.Empty, new[] { (1, 2) })));
    }
}
