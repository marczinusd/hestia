using System;
using System.Collections.Generic;
using FluentAssertions;
using Hestia.Model;
using Xunit;

namespace Test.Hestia.Model
{
    public class RepositorySnapshotTest
    {
        [Fact]
        public void WithShouldReturnNewObject()
        {
            var snapshot = new RepositorySnapshot(1,
                                                  string.Empty,
                                                  string.Empty,
                                                  default(DateTime),
                                                  new List<File>());

            snapshot.Should()
                    .NotBeSameAs(snapshot.With());
        }

        [Fact]
        public void WithShouldCorrectlyOverridePropertiesWithProvidedValues()
        {
            var snapshot = new RepositorySnapshot(1,
                                                  string.Empty,
                                                  string.Empty,
                                                  default(DateTime),
                                                  new List<File>());

            var newSnapshot = snapshot.With(new List<File>(),
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
        }
    }
}
