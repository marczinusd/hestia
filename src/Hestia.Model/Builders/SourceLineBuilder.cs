using System.Linq;
using Hestia.Model.Interfaces;
using LanguageExt;

namespace Hestia.Model.Builders
{
    public static class SourceLineBuilder
    {
        public static ISourceLine[] BuildSourceLineFromLineOfCode(string[] lines)
        {
            return lines.Select((line, index) => new SourceLine(index + 1,
                                                                line,
                                                                Option<ILineCoverageStats>.None,
                                                                Option<ILineGitStats>.None) as ISourceLine)
                        .ToArray();
        }
    }
}
