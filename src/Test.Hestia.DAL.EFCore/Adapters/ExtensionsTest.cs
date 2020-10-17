using System;
using System.Collections.Generic;
using FluentAssertions;
using Hestia.DAL.EFCore.Adapters;
using Hestia.DAL.EFCore.Entities;
using Xunit;

namespace Test.Hestia.DAL.EFCore.Adapters
{
    public class ExtensionsTest
    {
        private static readonly LineEntity LineEntity = new LineEntity("bla",
                                                                       true,
                                                                       1,
                                                                       2,
                                                                       "id",
                                                                       1);

        private static readonly FileEntity FileEntity = new FileEntity("path",
                                                                       1,
                                                                       2,
                                                                       3,
                                                                       new List<LineEntity> { LineEntity },
                                                                       "id");

        private static readonly RepositorySnapshotEntity SnapshotEntity =
            new RepositorySnapshotEntity(new List<FileEntity> { FileEntity },
                                         "hash",
                                         DateTime.MinValue,
                                         "name",
                                         "id",
                                         5,
                                         1);

        [Fact]
        public void MappersCreatesCorrectFileAdapterFromEntity()
        {
            var result = FileEntity.AsModel();

            result.Lines.Should()
                  .HaveCount(1);
            result.Id.Should()
                  .Be("id");
            result.Path.Should()
                  .Be("path");
            result.LifetimeAuthors.Should()
                  .Be(2);
            result.LifetimeChanges.Should()
                  .Be(1);
            result.CoveragePercentage.Should()
                  .Be(3);
        }

        [Fact]
        public void MappersCreatesCorrectLineAdapterFromEntity()
        {
            var result = LineEntity.AsModel();

            result.Content.Should()
                  .Be("bla");
            result.IsCovered.Should()
                  .BeTrue();
            result.NumberOfAuthors.Should()
                  .Be(1);
            result.NumberOfChanges.Should()
                  .Be(2);
            result.LineNumber.Should()
                  .Be(1);
        }

        [Fact]
        public void MappersCreatesCorrectSnapshotAdapterFromEntity()
        {
            var result = SnapshotEntity.AsModel();

            result.Files.Should()
                  .HaveCount(1);
            result.Id.Should()
                  .Be("id");
            result.Name.Should()
                  .Be("name");
            result.AtHash.Should()
                  .Be("hash");
            result.CommitDate.Should()
                  .Be(DateTime.MinValue);
            result.CommitRelativePosition.Should()
                  .Be(1);
            result.NumberOfCommits.Should()
                  .Be(5);
        }
    }
}
