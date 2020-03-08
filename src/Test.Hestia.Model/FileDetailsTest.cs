using System;
using System.Collections.Generic;
using FluentAssertions;
using Hestia.Model;
using Hestia.Model.Stats;
using LanguageExt;
using Xunit;

namespace Test.Hestia.Model
{
    public class FileDetailsTest
    {
        [Fact]
        public void FileDetailsShouldReflectUnderlyingFileProps()
        {
            var file = new File(String.Empty,
                                String.Empty,
                                String.Empty,
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
            var fileDetails = new FileDetails(file);

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
    }
}
