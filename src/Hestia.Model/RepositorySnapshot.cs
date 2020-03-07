using LanguageExt;

namespace Hestia.Model
{
    public class RepositorySnapshot
    {
        public RepositorySnapshot(long snapshotId,
                                  Directory rootDirectory,
                                  Option<string> pathToCoverageResultFile,
                                  Option<string> atHash)
        {
            RootDirectory = rootDirectory;
            PathToCoverageResultFile = pathToCoverageResultFile;
            AtHash = atHash;
            SnapshotId = snapshotId;
        }

        public Option<string> PathToCoverageResultFile { get; }

        public Option<string> AtHash { get; }

        public Directory RootDirectory { get; }

        public long SnapshotId { get; }

        public RepositorySnapshot With(Directory? directory = null,
                                       string? atHash = null,
                                       string? pathToCoverageResultFile = null) =>
            new RepositorySnapshot(SnapshotId,
                                   directory ?? RootDirectory,
                                   pathToCoverageResultFile ?? PathToCoverageResultFile,
                                   atHash ?? AtHash);

        public RepositoryIdentifier AsRepositoryIdentifier()
        {
            throw new System.NotImplementedException();
        }
    }
}
