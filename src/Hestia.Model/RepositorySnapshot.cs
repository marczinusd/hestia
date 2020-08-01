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
                                  IList<IFile> files,
                                  Option<string> pathToCoverageResultFile,
                                  Option<string> atHash,
                                  Option<DateTime> commitCreationDate,
                                  Option<string> repositoryName)
        {
            PathToCoverageResultFile = pathToCoverageResultFile;
            AtHash = atHash;
            CommitCreationDate = commitCreationDate;
            RepositoryName = repositoryName;
            Files = files;
            Id = id;
        }

        public Option<string> PathToCoverageResultFile { get; }

        public Option<DateTime> CommitCreationDate { get; }

        public Option<string> AtHash { get; }

        public Option<string> RepositoryName { get; }

        public IList<IFile> Files { get; }

        public string Id { get; }

        public IRepositorySnapshot With(IEnumerable<IFile>? files = null,
                                        string? atHash = null,
                                        string? pathToCoverageResultFile = null,
                                        DateTime? commitCreationDate = null,
                                        string? name = null) =>
            new RepositorySnapshot(Id,
                                   files?.ToList() ?? Files,
                                   pathToCoverageResultFile ?? PathToCoverageResultFile,
                                   atHash ?? AtHash,
                                   commitCreationDate ?? CommitCreationDate,
                                   name ?? RepositoryName);

        public SnapshotHeader AsHeader() => new SnapshotHeader(Id,
                                                               RepositoryName.Match(x => x, string.Empty),
                                                               AtHash.Match(x => x, string.Empty),
                                                               CommitCreationDate.Match(x => x, null));
    }
}
