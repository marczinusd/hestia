using LanguageExt;

namespace Hestia.Model.Interfaces
{
    public interface ISourceLine
    {
        int LineNumber { get; }

        string Text { get; }

        Option<ILineCoverageStats> LineCoverageStats { get; }

        Option<ILineGitStats> LineGitStats { get; }
    }
}
