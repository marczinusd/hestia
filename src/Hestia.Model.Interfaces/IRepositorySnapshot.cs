using System;
using System.Collections.Generic;
using LanguageExt;

namespace Hestia.Model.Interfaces
{
    public interface IRepositorySnapshot
    {
        Option<string> PathToCoverageResultFile { get; }

        Option<DateTime> CommitCreationDate { get; }

        Option<string> AtHash { get; }

        Option<string> RepositoryName { get; }

        string RootPath { get; }

        IList<IFile> Files { get; }

        IRepositorySnapshot With(IEnumerable<IFile>? files = null,
                                 string? atHash = null,
                                 string? pathToCoverageResultFile = null,
                                 DateTime? commitCreationDate = null,
                                 string? name = null,
                                 string? rootPath = null);
    }
}
