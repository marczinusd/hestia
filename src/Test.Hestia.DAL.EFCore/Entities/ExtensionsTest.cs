using System;
using System.Collections.Generic;
using FluentAssertions;
using Hestia.DAL.EFCore.Entities;
using Hestia.Model;
using Hestia.Model.Interfaces;
using Hestia.Model.Stats;
using LanguageExt;
using Xunit;
using static LanguageExt.Prelude;

namespace Test.Hestia.DAL.EFCore.Entities
{
    public class ExtensionsTest
    {
        private static readonly SourceLine Line = new SourceLine(1,
                                                                 "bla",
                                                                 new LineCoverageStats(true),
                                                                 new LineGitStats(1, 2, 3));

        private static readonly File File = new File("filename",
                                                     "ext",
                                                     "path",
                                                     new List<ISourceLine> { Line },
                                                     new FileGitStats(1, 2),
                                                     new FileCoverageStats(new FileCoverage("path", new[] { (1, 1) })));

        private static readonly RepositorySnapshot Snapshot = new RepositorySnapshot("id",
            new List<IFile> { File },
            Option<string>.None,
            Some("hash"),
            Some(DateTime.MinValue),
            Some("name"));

        [Fact]
        public void FileIsCorrectlyMappedToEntityTest()
        {
            var entity = File.AsEntity();

            entity.Path
                  .Should()
                  .Be(File.FullPath);
            entity.LifetimeAuthors
                  .Should()
                  .Be(File.LifetimeAuthors);
            entity.LifetimeChanges
                  .Should()
                  .Be(File.LifetimeChanges);
            entity.CoveragePercentage
                  .Should()
                  .Be(File.CoveragePercentage);
            entity.Id.Should()
                  .BeNull();
            entity.Lines.Should()
                  .HaveCount(1);
            entity.Parent.Should()
                  .BeNull();
        }

        [Fact]
        public void RepositorySnapshotIsCorrectlyMappedToEntityTest()
        {
            var entity = Snapshot.AsEntity();

            entity.Files
                  .Should()
                  .HaveCount(Snapshot.Files.Count);
            entity.AtHash
                  .Should()
                  .Be(Snapshot.AtHash.Match(x => x, string.Empty));
            entity.CommitDate
                  .Should()
                  .Be(Snapshot.CommitCreationDate.Match(x => x, DateTime.MaxValue));
        }

        [Fact]
        public void LineIsCorrectlyMappedToEntity()
        {
            var entity = Line.AsEntity();

            entity.Content.Should()
                  .Be("bla");
            entity.LineNumber.Should()
                  .Be(1);
            entity.Parent.Should()
                  .BeNull();
            entity.IsCovered.Should()
                  .BeTrue();
            entity.NumberOfAuthors.Should()
                  .Be(3);
            entity.NumberOfChanges.Should()
                  .Be(2);
        }
    }
}
