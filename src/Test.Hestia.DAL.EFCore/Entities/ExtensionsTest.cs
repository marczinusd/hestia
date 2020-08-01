using System;
using System.Collections.Generic;
using System.Linq;
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
        [Fact]
        public void FileIsCorrectlyMappedToEntityTest()
        {
            var file = new File("filename",
                                "ext",
                                "path",
                                Enumerable.Empty<ISourceLine>()
                                          .ToList(),
                                new FileGitStats(1, 2),
                                new FileCoverageStats(new FileCoverage("path", new[] { (1, 1) })));

            var entity = file.AsEntity();

            entity.Path
                  .Should()
                  .Be(file.FullPath);
            entity.LifetimeAuthors
                  .Should()
                  .Be(file.LifetimeAuthors);
            entity.LifetimeChanges
                  .Should()
                  .Be(file.LifetimeChanges);
            entity.CoveragePercentage
                  .Should()
                  .Be(file.CoveragePercentage);
        }

        [Fact]
        public void RepositorySnapshotIsCorrectlyMappedToEntityTest()
        {
            var snapshot = new RepositorySnapshot("id",
                                                  new List<IFile>(),
                                                  Option<string>.None,
                                                  Some("hash"),
                                                  Some(DateTime.MinValue),
                                                  Some("name"));

            var entity = snapshot.AsEntity();

            entity.Files
                  .Should()
                  .HaveCount(snapshot.Files.Count);
            entity.AtHash
                  .Should()
                  .Be(snapshot.AtHash.Match(x => x, string.Empty));
            entity.HashDate
                  .Should()
                  .Be(snapshot.CommitCreationDate.Match(x => x, DateTime.MaxValue));
        }
    }
}
