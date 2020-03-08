using System;
using System.Collections.Generic;
using System.IO;
using FluentAssertions;
using Hestia.Model;
using Hestia.Model.Stats;
using LanguageExt;
using Xunit;
using File = Hestia.Model.File;

namespace Test.Hestia.Model
{
    public class FileTest
    {
        [Fact]
        public void WhenCreatesANewFileObject()
        {
            var file = new File(string.Empty,
                                string.Empty,
                                string.Empty,
                                new List<SourceLine>(),
                                Option<FileGitStats>.None,
                                Option<FileCoverageStats>.None);

            file.Should()
                .NotBeSameAs(file.With());
        }

        [Fact]
        public void WhenShouldCreateInstanceWithProvidedParams()
        {
            var file = new File(string.Empty,
                                string.Empty,
                                string.Empty,
                                new List<SourceLine>(),
                                Option<FileGitStats>.None,
                                Option<FileCoverageStats>.None);

            var newFile = file.With(new List<SourceLine>
                                    {
                                        new SourceLine(1,
                                                       string.Empty,
                                                       Option<LineCoverageStats>.None,
                                                       Option<LineGitStats>.None),
                                    },
                                    new FileGitStats(1, 1),
                                    new FileCoverageStats(new FileCoverage(string.Empty,
                                                                           new List<(int lineNumber, int hitCount)
                                                                           >())));

            newFile.Content.Should()
                   .HaveCount(1);
            newFile.GitStats.Match(x => x.LifetimeAuthors,
                                   () => 0)
                   .Should()
                   .Be(1);
            newFile.CoverageStats.Match(x => x.Coverage,
                                        () => null)
                   .Should()
                   .NotBeNull();
        }

        [Fact]
        public void AsFileDetailsCreatesEquivalentFileDetailsObject()
        {
            var file = new File(string.Empty,
                                string.Empty,
                                string.Empty,
                                new List<SourceLine>
                                {
                                    new SourceLine(1,
                                                   string.Empty,
                                                   Option<LineCoverageStats>.None,
                                                   Option<LineGitStats>.None),
                                },
                                new FileGitStats(1, 1),
                                new FileCoverageStats(new FileCoverage(string.Empty,
                                                                       new List<(int lineNumber, int hitCount)
                                                                       >())));
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
                              () => null)
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
                                new List<SourceLine>(),
                                Option<FileGitStats>.None,
                                Option<FileCoverageStats>.None);

            file.FullPath
                .Should()
                .BeEquivalentTo($"{file.Path}{Path.DirectorySeparatorChar}{file.Filename}");
        }
    }
}
