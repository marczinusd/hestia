using System;
using LanguageExt;

namespace Hestia.Model
{
    public class RepositorySnapshot
    {
        public RepositorySnapshot(long snapshotId,
                                  Directory rootDirectory,
                                  Option<string> pathToCoverageResultFile,
                                  Option<string> atHash,
                                  Option<DateTime> commitCreationDate)
        {
            RootDirectory = rootDirectory;
            PathToCoverageResultFile = pathToCoverageResultFile;
            AtHash = atHash;
            CommitCreationDate = commitCreationDate;
            SnapshotId = snapshotId;
        }

        public Option<string> PathToCoverageResultFile { get; }

        public Option<DateTime> CommitCreationDate { get; }

        public Option<string> AtHash { get; }

        public Directory RootDirectory { get; }

        public long SnapshotId { get; }

        public RepositorySnapshot With(Directory? directory = null,
                                       string? atHash = null,
                                       string? pathToCoverageResultFile = null,
                                       DateTime? commitCreationDate = null) =>
            new RepositorySnapshot(SnapshotId,
                                   directory ?? RootDirectory,
                                   pathToCoverageResultFile ?? PathToCoverageResultFile,
                                   atHash ?? AtHash,
                                   commitCreationDate ?? CommitCreationDate);

        public RepositoryIdentifier AsRepositoryIdentifier()
        {
            throw new NotImplementedException();
        }
    }
}
