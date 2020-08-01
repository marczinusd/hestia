using System.Collections.Generic;
using LanguageExt;

namespace Hestia.Model.Interfaces
{
    public interface IFile
    {
        IList<ISourceLine> Content { get; }

        string FullPath { get; }

        string Path { get; }

        string Filename { get; }

        string Extension { get; }

        decimal CoveragePercentage { get; }

        int LifetimeAuthors { get; }

        int LifetimeChanges { get; }

        Option<IFileGitStats> GitStats { get; }

        Option<IFileCoverageStats> CoverageStats { get; }

        IFile With(IList<ISourceLine>? content = null,
                   IFileGitStats? gitStats = null,
                   IFileCoverageStats? coverageStats = null);
    }
}
