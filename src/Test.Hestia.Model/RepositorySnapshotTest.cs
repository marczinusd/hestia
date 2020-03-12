using System;
using System.Collections.Generic;
using FluentAssertions;
using Hestia.Model;
using Test.Hestia.Model.TestData;
using Xunit;

namespace Test.Hestia.Model
{
    public class RepositorySnapshotTest
    {
        [Fact]
        public void WithShouldReturnNewObject()
        {
            var snapshot = new RepositorySnapshot(1,
                                                  new List<File>(),
                                                  string.Empty,
                                                  string.Empty,
                                                  default(DateTime));

            snapshot.Should()
                    .NotBeSameAs(snapshot.With());
        }

        [Fact]
        public void WithShouldCorrectlyOverridePropertiesWithProvidedValues()
        {
            var snapshot = new RepositorySnapshot(1,
                                                  new List<File>(),
                                                  string.Empty,
                                                  string.Empty,
                                                  default(DateTime));

            var newSnapshot = snapshot.With(MockRepo.CreateFiles(3),
                                            "hash",
                                            "path",
                                            DateTime.Today);

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
            newSnapshot.Files
                       .Should()
                       .HaveCount(3);
        }
    }
}
