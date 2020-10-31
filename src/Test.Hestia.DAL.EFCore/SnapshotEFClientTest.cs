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
using File = Hestia.DAL.EFCore.Entities.File;

namespace Test.Hestia.DAL.EFCore
{
    public class SnapshotEFClientTest
    {
        private const string SeededSnapshotId = "repoid";
        private const string SeededFileId = "fileId";

        private static readonly Snapshot SeededRepo = new Snapshot(new List<File>
                                                                   {
                                                                       new File("path",
                                                                                    1,
                                                                                    2,
                                                                                    100,
                                                                                    new List<Line>
                                                                                    {
                                                                                        new Line("blabla",
                                                                                            true,
                                                                                            1,
                                                                                            2,
                                                                                            "id",
                                                                                            1,
                                                                                            null)
                                                                                    },
                                                                                    SeededFileId,
                                                                                    null)
                                                                   },
                                                                   "someHash",
                                                                   DateTime.MinValue,
                                                                   "someRepo",
                                                                   SeededSnapshotId,
                                                                   5,
                                                                   2);

        private static readonly RepositorySnapshot NewSnapshot =
            new RepositorySnapshot("newid",
                                   "somePath",
                                   new List<IFile>(),
                                   "somename",
                                   "coveragePath",
                                   "hash",
                                   DateTime.MinValue,
                                   1,
                                   0);

        public SnapshotEFClientTest() => SeedDb();

        private static DbContextOptions<HestiaContext> Options =>
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

            context.Snapshots.Count()
                   .Should()
                   .Be(2);
            context.Snapshots.ToList()[1]
                   .Name
                   .Should()
                   .Be("somename");
            context.SourceLines
                   .Should()
                   .HaveCount(1);
        }

        [Fact]
        public void InsertSnapshotSyncShouldPersistAsExpected()
        {
            using var context = new HestiaContext(Options);
            var client = new SnapshotEFClient(context);

            client.InsertSnapshotSync(NewSnapshot);

            context.Snapshots.Count()
                   .Should()
                   .Be(2);
            context.Snapshots.ToList()[1]
                   .Name
                   .Should()
                   .Be("somename");
            context.SourceLines
                   .Should()
                   .HaveCount(1);
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

            client.GetFileDetails(SeededFileId)
                  .Match(x => x, () => null)
                  ?.Id.Should()
                  .Be(SeededFileId);
        }

        [Fact]
        public void GetFileDetailsShouldReturnNoneIfFileWasNotFound()
        {
            using var context = new HestiaContext(Options);
            var client = new SnapshotEFClient(context);

            client.GetFileDetails("invalid")
                  .IsNone
                  .Should()
                  .BeTrue();
        }

        [Fact]
        public void ClientShouldDisposeDbContext()
        {
            var context = new HestiaContextSpy(Options);
            var client = new SnapshotEFClient(context);

            client.Dispose();

            context.IsDisposed.Should()
                   .BeTrue();
        }

        [Fact]
        public void DisposeCanBeCalledSafelyIfContextIsNull()
        {
            var client = new SnapshotEFClient(null);

            Action act = () => client.Dispose();

            act.Should()
               .NotThrow<NullReferenceException>();
        }

        [Fact]
        public void GetLinesForFileRetrievesTheExpectedLines()
        {
            using var context = new HestiaContext(Options);
            var client = new SnapshotEFClient(context);

            var lines = client.GetLinesForFile(SeededFileId);

            lines.Should()
                 .HaveCount(1);
        }

        [Fact]
        public void GetAllFilesForSnapshotRetrievesAllFilesForSnapshotId()
        {
            using var context = new HestiaContext(Options);
            var client = new SnapshotEFClient(context);

            var files = client.GetAllFilesForSnapshot(SeededSnapshotId);

            files.First()
                 .Id.Should()
                 .Be(SeededFileId);
        }

        private void SeedDb()
        {
            using var context = new HestiaContext(Options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.Snapshots.Add(SeededRepo);

            context.SaveChanges();
        }

        private class HestiaContextSpy : HestiaContext
        {
            public HestiaContextSpy(DbContextOptions options)
                : base(options)
            {
            }

            public bool IsDisposed { get; set; }

            public override void Dispose() => IsDisposed = true;
        }
    }
}
