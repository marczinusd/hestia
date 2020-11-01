using System.Collections.Generic;
using System.IO;
using FluentAssertions;
using Hestia.Model;
using Hestia.Model.Interfaces;
using Hestia.Model.Stats;
using LanguageExt;
using Test.Hestia.Utils.TestData;
using Xunit;
using File = Hestia.Model.File;

namespace Test.Hestia.Model
{
    public class FileTest
    {
        [Fact]
        public void WhenCreatesANewFileObject()
        {
            var file = MockRepo.CreateFile();

            file.Should()
                .NotBeSameAs(file.With());
        }

        [Fact]
        public void IdShouldAlwaysBeEmptyString() =>
            MockRepo.CreateFile()
                    .Id.Should()
                    .BeEmpty();

        [Fact]
        public void WhenShouldCreateInstanceWithProvidedParams()
        {
            var file = MockRepo.CreateFile();

            var newFile = file.With(new List<ISourceLine>
                                    {
                                        new SourceLine(1,
                                                       string.Empty,
                                                       Option<ILineCoverageStats>.None,
                                                       Option<ILineGitStats>.None)
                                    },
                                    new FileGitStats(1, 1),
                                    new FileCoverageStats(new FileCoverage(string.Empty,
                                                                           new List<(int lineNumber, int hitCount, bool
                                                                               branch, string conditionCoverage)
                                                                           >())));

            newFile.Content.Should()
                   .HaveCount(1);
            newFile.GitStats.Match(x => x.LifetimeAuthors,
                                   () => 0)
                   .Should()
                   .Be(1);
            newFile.CoverageStats.Match(x => x.Coverage,
                                        () => default)
                   .Should()
                   .NotBeNull();
        }

        [Fact]
        public void AsFileDetailsCreatesEquivalentFileDetailsObject()
        {
            var file = MockRepo.CreateFile();
            var fileDetails = file.AsFileDetails();

            fileDetails.Content.Should()
                       .BeEquivalentTo(file.Content);
            fileDetails.Extension.Should()
                       .BeEquivalentTo(file.Extension);
            fileDetails.Filename.Should()
                       .BeEquivalentTo(file.Filename);
            fileDetails.Path.Should()
                       .BeEquivalentTo(file.Path);
            fileDetails.CoverageStats
                       .Match(x => x.Coverage.FileName,
                              () => default)
                       .Should()
                       .BeEmpty();
            fileDetails.GitStats
                       .Match(x => x.LifetimeAuthors,
                              () => 0)
                       .Should()
                       .Be(1);
        }

        [Fact]
        public void FullPathCorrectlyAssemblesFullFilePath()
        {
            var file = new File("some.cs",
                                string.Empty,
                                "dir",
                                new List<ISourceLine>(),
                                Option<IFileGitStats>.None,
                                Option<IFileCoverageStats>.None);

            file.FullPath
                .Should()
                .BeEquivalentTo($"{file.Path}{Path.DirectorySeparatorChar}{file.Filename}");
        }
    }
}
