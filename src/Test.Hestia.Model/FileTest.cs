using System;
using System.Collections.Generic;
using FluentAssertions;
using Hestia.Model;
using Hestia.Model.Stats;
using LanguageExt;
using Xunit;

namespace Test.Hestia.Model
{
    public class FileTest
    {
        [Fact]
        public void WhenCreatesANewFileObject()
        {
            var file = new File(String.Empty,
                                String.Empty,
                                String.Empty,
                                new List<SourceLine>(),
                                Option<FileGitStats>.None,
                                Option<FileCoverageStats>.None);

            file.Should()
                .NotBeSameAs(file.With());
        }

        [Fact]
        public void WhenShouldCreateInstanceWithProvidedParams()
        {
            var file = new File(String.Empty,
                                String.Empty,
                                String.Empty,
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
    }
}
