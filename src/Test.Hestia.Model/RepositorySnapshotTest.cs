using System;
using System.Collections.Generic;
using FluentAssertions;
using Hestia.Model;
using Hestia.Model.Interfaces;
using Test.Hestia.Utils.TestData;
using Xunit;

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
                                                  string.Empty);

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
                                                  string.Empty);

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
