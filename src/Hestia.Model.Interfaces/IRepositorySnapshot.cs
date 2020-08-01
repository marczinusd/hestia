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

        IList<IFile> Files { get; }
    }
}
