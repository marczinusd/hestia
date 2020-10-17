using System;
using System.Collections.Generic;
using FluentAssertions;
using Hestia.Model;
using Hestia.Model.Interfaces;
using Test.Hestia.Utils.TestData;
using Xunit;
using static LanguageExt.Prelude;

namespace Test.Hestia.Model
{
    public class RepositorySnapshotTest
    {
        [Fact]
        public void WithShouldReturnNewObject()
        {
            var snapshot = new RepositorySnapshot(string.Empty,
                                                  new List<IFile>(),
                                                  string.Empty,
                                                  string.Empty,
                                                  default(DateTime),
                                                  string.Empty,
                                                  string.Empty,
                                                  0,
                                                  0);

            snapshot.Should()
                    .NotBeSameAs(snapshot.With());
        }

        [Fact]
        public void WithShouldCorrectlyOverridePropertiesWithProvidedValues()
        {
            var snapshot = new RepositorySnapshot(string.Empty,
                                                  new List<IFile>(),
                                                  string.Empty,
                                                  string.Empty,
                                                  default(DateTime),
                                                  string.Empty,
                                                  string.Empty,
                                                  0,
                                                  0);

            var newSnapshot = snapshot.With(MockRepo.CreateFiles(3),
                                            "hash",
                                            "path",
                                            DateTime.Today,
                                            "newName");

            newSnapshot.AtHash
                       .Match(x => x, () => string.Empty)
                       .Should()
                       .Be("hash");
            newSnapshot.PathToCoverageResultFile
                       .Match(x => x, () => string.Empty)
                       .Should()
                       .Be("path");
            newSnapshot.CommitCreationDate
                       .Match(x => x, () => default)
                       .Should()
                       .NotBe(default);
            newSnapshot.RepositoryName
                       .Match(x => x, () => default)
                       .Should()
                       .Be("newName");
            newSnapshot.Files
                       .Should()
                       .HaveCount(3);
        }

        [Fact]
        public void AsHeaderShouldCreateExpectedRepositorySnapshotRepresentation()
        {
            var snapshot = new RepositorySnapshot("id",
                                                  new List<IFile>(),
                                                  Some("path"),
                                                  Some("hash"),
                                                  Some(DateTime.MaxValue),
                                                  Some("name"),
                                                  "path",
                                                  0,
                                                  0);

            var header = snapshot.AsHeader();

            header.Id
                  .Should()
                  .Be(snapshot.Id);
            header.Name
                  .Should()
                  .Be(snapshot.RepositoryName.Match(x => x, string.Empty));
            header.AtHash
                  .Should()
                  .Be(snapshot.AtHash.Match(x => x, string.Empty));
            header.CommitDate
                  .Should()
                  .Be(snapshot.CommitCreationDate.Match(x => x, DateTime.MinValue));
        }
    }
}
