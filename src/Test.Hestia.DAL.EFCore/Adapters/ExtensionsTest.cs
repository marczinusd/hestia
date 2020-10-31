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
        private static readonly Line Line = new Line("bla",
                                                     true,
                                                     1,
                                                     2,
                                                     "id",
                                                     1,
                                                     null);

        private static readonly File File = new File("path",
                                                     1,
                                                     2,
                                                     3,
                                                     new List<Line> { Line },
                                                     "id",
                                                     null);

        private static readonly Snapshot Snapshot =
            new Snapshot(new List<File> { File },
                         "hash",
                         DateTime.MinValue,
                         "name",
                         "id",
                         5,
                         1);

        [Fact]
        public void MappersCreatesCorrectFileAdapterFromEntity()
        {
            var result = File.AsModel();

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
            var result = Line.AsModel();

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
            var result = Snapshot.AsModel();

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
