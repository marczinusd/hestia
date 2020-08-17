using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Hestia.DAL.EFCore;
using Hestia.DAL.EFCore.Entities;
using Hestia.Model;
using Hestia.Model.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Reactive.Testing;
using Xunit;

namespace Test.Hestia.DAL.EFCore
{
    public class SnapshotEFClientTest
    {
        private const string SeededSnapshotId = "repoid";
        private const string SeededFileId = "fileId";

        private static readonly RepositorySnapshotEntity SeededRepo = new RepositorySnapshotEntity(new List<FileEntity>
            {
                new FileEntity("path",
                               1,
                               2,
                               100,
                               new List<LineEntity>
                               {
                                   new LineEntity("blabla",
                                                  true,
                                                  1,
                                                  2,
                                                  "id"),
                               },
                               SeededFileId),
            },
            "someHash",
            DateTime.MinValue,
            "someRepo",
            SeededSnapshotId);

        private static readonly RepositorySnapshot NewSnapshot =
            new RepositorySnapshot("newid",
                                   new List<IFile>(),
                                   "coveragePath",
                                   "hash",
                                   DateTime.MinValue,
                                   "somename");

        public SnapshotEFClientTest() => SeedDb();

        private DbContextOptions<HestiaContext> Options =>
            new DbContextOptionsBuilder<HestiaContext>()
                .UseSqlite("Data Source=hestia.test.db")
                .Options;

        [Fact]
        public void InsertSnapshotShouldPersistAsExpected()
        {
            using var context = new HestiaContext(Options);
            var scheduler = new TestScheduler();
            var client = new SnapshotEFClient(context);

            scheduler.Start(() => client.InsertSnapshot(NewSnapshot));

            context.Snapshots
                   .Should()
                   .HaveCount(2);
            context.Snapshots.ToList()[1]
                   .Name
                   .Should()
                   .Be("somename");
        }

        [Fact]
        public void GetSnapshotByIdShouldReturnSeededSnapshot()
        {
            using var context = new HestiaContext(Options);
            var client = new SnapshotEFClient(context);

            client.GetSnapshotById(SeededSnapshotId)
                  .Match(x => x, () => null)
                  ?.Id.Should()
                  .Be(SeededSnapshotId);
        }

        [Fact]
        public void GetSnapshotByIdShouldReturnNoneIfSnapshotWasNotFound()
        {
            using var context = new HestiaContext(Options);
            var client = new SnapshotEFClient(context);

            client.GetSnapshotById("invalid")
                  .IsNone
                  .Should()
                  .BeTrue();
        }

        [Fact]
        public void GetAllSnapshotsShouldReturnTheSeededSnapshots()
        {
            using var context = new HestiaContext(Options);
            var client = new SnapshotEFClient(context);

            client.GetAllSnapshotsHeaders()
                  .First()
                  .Id
                  .Should()
                  .Be(SeededSnapshotId);
        }

        [Fact]
        public void GetFileDetailsShouldReturnSeededFileDetails()
        {
            using var context = new HestiaContext(Options);
            var client = new SnapshotEFClient(context);

            client.GetFileDetails(SeededFileId, SeededSnapshotId)
                  .Match(x => x, () => null)
                  ?.Id.Should()
                  .Be(SeededFileId);
        }

        [Fact]
        public void GetFileDetailsShouldReturnNoneIfFileWasNotFound()
        {
            using var context = new HestiaContext(Options);
            var client = new SnapshotEFClient(context);

            client.GetFileDetails("invalid", SeededSnapshotId)
                  .IsNone
                  .Should()
                  .BeTrue();
        }

        private void SeedDb()
        {
            using var context = new HestiaContext(Options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.Snapshots.Add(SeededRepo);

            context.SaveChanges();
        }
    }
}
