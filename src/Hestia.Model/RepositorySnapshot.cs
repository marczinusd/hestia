using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;

namespace Hestia.Model
{
    public class RepositorySnapshot
    {
        public RepositorySnapshot(long snapshotId,
                                  IList<File> files,
                                  Option<string> pathToCoverageResultFile,
                                  Option<string> atHash,
                                  Option<DateTime> commitCreationDate)
        {
            PathToCoverageResultFile = pathToCoverageResultFile;
            AtHash = atHash;
            CommitCreationDate = commitCreationDate;
            Files = files;
            SnapshotId = snapshotId;
        }

        public Option<string> PathToCoverageResultFile { get; }

        public Option<DateTime> CommitCreationDate { get; }

        public Option<string> AtHash { get; }

        public IList<File> Files { get; }

        public long SnapshotId { get; }

        public RepositorySnapshot With(IEnumerable<File>? files = null,
                                       string? atHash = null,
                                       string? pathToCoverageResultFile = null,
                                       DateTime? commitCreationDate = null) =>
            new RepositorySnapshot(SnapshotId,
                                   files?.ToList() ?? Files,
                                   pathToCoverageResultFile ?? PathToCoverageResultFile,
                                   atHash ?? AtHash,
                                   commitCreationDate ?? CommitCreationDate);
    }
}
