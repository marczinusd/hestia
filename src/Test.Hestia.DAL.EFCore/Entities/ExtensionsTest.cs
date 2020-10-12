using System;
using System.Collections.Generic;
using FluentAssertions;
using Hestia.DAL.EFCore.Entities;
using Hestia.Model;
using Hestia.Model.Interfaces;
using Hestia.Model.Stats;
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

        private static readonly SourceLine LineWithNones = new SourceLine(1,
                                                                          "bla",
                                                                          None,
                                                                          None);

        private static readonly File File = new File("filename",
                                                     "ext",
                                                     "path",
                                                     new List<ISourceLine> { Line },
                                                     new FileGitStats(1, 2),
                                                     new FileCoverageStats(new FileCoverage("path", new[] { (1, 1) })));

        private static readonly RepositorySnapshot Snapshot = new RepositorySnapshot("id",
            new List<IFile> { File },
            Some("path"),
            Some("hash"),
            Some(DateTime.MinValue),
            Some("name"),
            "somePath");

        private static readonly File FileWithNones = new File("filename",
                                                              "ext",
                                                              "path",
                                                              new List<ISourceLine> { LineWithNones },
                                                              None,
                                                              None);

        private static readonly RepositorySnapshot SnapshotWithNones = new RepositorySnapshot("id",
            new List<IFile> { FileWithNones },
            None,
            None,
            None,
            None,
            string.Empty);

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
        public void FileWithNonesIsCorrectlyMappedToEntityTest()
        {
            var entity = FileWithNones.AsEntity();

            entity.Path
                  .Should()
                  .Be(File.FullPath);
            entity.LifetimeAuthors
                  .Should()
                  .Be(-1);
            entity.LifetimeChanges
                  .Should()
                  .Be(-1);
            entity.CoveragePercentage
                  .Should()
                  .Be(-1m);
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
        public void RepositorySnapshotWithNonesIsCorrectlyMappedToEntityTest()
        {
            var entity = SnapshotWithNones.AsEntity();

            entity.Files
                  .Should()
                  .HaveCount(Snapshot.Files.Count);
            entity.AtHash
                  .Should()
                  .BeEmpty();
            entity.CommitDate
                  .Should()
                  .Be(DateTime.MinValue);
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
            entity.Id.Should()
                  .BeNull();
            entity.Parent.Should()
                  .BeNull();
        }

        [Fact]
        public void LineWithNonesIsCorrectlyMappedToEntity()
        {
            var entity = LineWithNones.AsEntity();

            entity.Content.Should()
                  .Be("bla");
            entity.LineNumber.Should()
                  .Be(1);
            entity.Parent.Should()
                  .BeNull();
            entity.IsCovered.Should()
                  .BeFalse();
            entity.NumberOfAuthors.Should()
                  .Be(0);
            entity.NumberOfChanges.Should()
                  .Be(0);
            entity.Id.Should()
                  .BeNull();
            entity.Parent.Should()
                  .BeNull();
        }
    }
}
