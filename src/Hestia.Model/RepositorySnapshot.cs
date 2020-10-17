using System;
using System.Collections.Generic;
using System.Linq;
using Hestia.Model.Interfaces;
using LanguageExt;

namespace Hestia.Model
{
    public class RepositorySnapshot : IRepositorySnapshot
    {
        public RepositorySnapshot(string id,
                                  string rootPath,
                                  IList<IFile> files,
                                  Option<string> repositoryName,
                                  Option<string> pathToCoverageResultFile,
                                  Option<string> atHash,
                                  Option<DateTime> commitCreationDate,
                                  Option<int> commitRelativePosition,
                                  Option<int> numberOfCommitsOnBranch)
        {
            PathToCoverageResultFile = pathToCoverageResultFile;
            AtHash = atHash;
            CommitCreationDate = commitCreationDate;
            RepositoryName = repositoryName;
            RootPath = rootPath;
            CommitRelativePosition = commitRelativePosition;
            NumberOfCommitsOnBranch = numberOfCommitsOnBranch;
            Files = files;
            Id = id;
        }

        public string Id { get; }

        public Option<string> PathToCoverageResultFile { get; }

        public Option<DateTime> CommitCreationDate { get; }

        public Option<string> AtHash { get; }

        public Option<string> RepositoryName { get; }

        public Option<int> CommitRelativePosition { get; }

        public Option<int> NumberOfCommitsOnBranch { get; }

        public string RootPath { get; }

        public IList<IFile> Files { get; }

        public IRepositorySnapshot With(IEnumerable<IFile>? files = null,
                                        string? atHash = null,
                                        string? pathToCoverageResultFile = null,
                                        DateTime? commitCreationDate = null,
                                        string? name = null,
                                        string? rootPath = null,
                                        int? commitRelativePosition = null,
                                        int? numberOfCommitsOnBranch = null) =>
            new RepositorySnapshot(Id,
                                   rootPath ?? RootPath,
                                   files?.ToList() ?? Files,
                                   name ?? RepositoryName,
                                   pathToCoverageResultFile ?? PathToCoverageResultFile,
                                   atHash ?? AtHash,
                                   commitCreationDate ?? CommitCreationDate,
                                   commitRelativePosition ?? CommitRelativePosition,
                                   numberOfCommitsOnBranch ?? NumberOfCommitsOnBranch);

        public SnapshotHeader AsHeader() => new SnapshotHeader(Id,
                                                               RepositoryName.Match(x => x, string.Empty),
                                                               AtHash.Match(x => x, string.Empty),
                                                               CommitCreationDate.Match(x => x, null));
    }
}
