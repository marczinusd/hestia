using System;
using System.IO;
using FluentAssertions;
using Hestia.Model.Builders;
using Moq;
using Test.Hestia.Model.TestData;
using Xunit;

namespace Test.Hestia.Model.Builders
{
    public class RepositorySnapshotBuilderTest
    {
        private static readonly string DirPath = Path.Join("C:", "temp");

        [Fact]
        public void RepositorySnapshotBuilderBuildsExpectedStructureFromArguments()
        {
            var args = new RepositorySnapshotBuilderArguments(1,
                                                              DirPath,
                                                              string.Empty,
                                                              new[] { ".cs" },
                                                              "lcov.info",
                                                              "hash",
                                                              default(DateTime),
                                                              MockRepo.CreateDiskIOWrapperMock().Object,
                                                              Mock.Of<IPathValidator>());

            var snapshot = args.Build();

            snapshot.Files
                    .Should()
                    .HaveCount(2);
            snapshot.AtHash.Match(x => x, () => string.Empty)
                    .Should()
                    .Be("hash");
            snapshot.SnapshotId
                    .Should()
                    .Be(1);
            snapshot.CommitCreationDate.Match(x => x, DateTime.Today)
                    .Should()
                    .Be(default);
        }
    }
}
