using Hestia.Model.Wrappers;
using LanguageExt;

namespace Hestia.Model.Builders
{
    public class RepositorySnapshotBuilderArguments
    {
        public RepositorySnapshotBuilderArguments(long snapshotId,
                                                  string rootPath,
                                                  string sourceRoot,
                                                  string[] sourceExtensions,
                                                  string coveragePath,
                                                  Option<string> atHash,
                                                  IDiskIOWrapper diskIoWrapper,
                                                  IPathValidator pathValidator)
        {
            SnapshotId = snapshotId;
            RootPath = rootPath;
            SourceRoot = sourceRoot;
            SourceExtensions = sourceExtensions;
            CoveragePath = coveragePath;
            AtHash = atHash;
            DiskIoWrapper = diskIoWrapper;
            PathValidator = pathValidator;
        }

        public long SnapshotId { get; }

        public string RootPath { get; }

        public string SourceRoot { get; }

        public string[] SourceExtensions { get; }

        public string CoveragePath { get; }

        public Option<string> AtHash { get; }

        public IDiskIOWrapper DiskIoWrapper { get; }

        public IPathValidator PathValidator { get; }

        public RepositorySnapshotBuilderArguments With(long? snapshotId = null, string hash = null) =>
            new RepositorySnapshotBuilderArguments(snapshotId ?? SnapshotId,
                                                   RootPath,
                                                   SourceRoot,
                                                   SourceExtensions,
                                                   CoveragePath,
                                                   hash ?? AtHash,
                                                   DiskIoWrapper,
                                                   PathValidator);
    }
}
