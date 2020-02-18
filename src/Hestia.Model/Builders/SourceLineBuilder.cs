using System.Linq;
using Hestia.Model.Stats;
using LanguageExt;

namespace Hestia.Model.Builders
{
    public static class SourceLineBuilder
    {
        public static SourceLine[] BuildSourceLineFromLineOfCode(string[] lines)
        {
            return lines.Select((line, index) => new SourceLine(index,
                                                                line,
                                                                Option<LineCoverageStats>.None,
                                                                Option<LineGitStats>.None)).ToArray();
        }
    }
}
