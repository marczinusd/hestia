using Hestia.Model.Wrappers;
using LanguageExt;

namespace Hestia.Model.Builders
{
    public class RepositorySnapshotBuilderArguments
    {
        public RepositorySnapshotBuilderArguments(long repoId,
                                                  string rootPath,
                                                  string sourceRoot,
                                                  string[] sourceExtensions,
                                                  string coveragePath,
                                                  Option<string> atHash,
                                                  IDiskIOWrapper diskIoWrapper,
                                                  IPathValidator pathValidator)
        {
            RepoId = repoId;
            RootPath = rootPath;
            SourceRoot = sourceRoot;
            SourceExtensions = sourceExtensions;
            CoveragePath = coveragePath;
            AtHash = atHash;
            DiskIoWrapper = diskIoWrapper;
            PathValidator = pathValidator;
        }

        public long RepoId { get; }

        public string RootPath { get; }

        public string SourceRoot { get; }

        public string[] SourceExtensions { get; }

        public string CoveragePath { get; }

        public Option<string> AtHash { get; }

        public IDiskIOWrapper DiskIoWrapper { get; }

        public IPathValidator PathValidator { get; }

        public RepositorySnapshotBuilderArguments With(string hash) =>
            new RepositorySnapshotBuilderArguments(RepoId,
                                                   RootPath,
                                                   SourceRoot,
                                                   SourceExtensions,
                                                   CoveragePath,
                                                   hash ?? AtHash,
                                                   DiskIoWrapper,
                                                   PathValidator);
    }
}
