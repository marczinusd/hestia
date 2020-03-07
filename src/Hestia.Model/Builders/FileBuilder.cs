using System.IO;
using System.Linq;
using Hestia.Model.Stats;
using Hestia.Model.Wrappers;
using LanguageExt;

namespace Hestia.Model.Builders
{
    public class FileBuilder
    {
        public static File BuildFileFromPath(string filePath, IDiskIOWrapper diskIoWrapper)
        {
            var fileContent = diskIoWrapper.ReadAllLinesFromFile(filePath);

            return new File(Path.GetFileName(filePath) ?? throw new FileBuilderException(filePath),
                            Path.GetExtension(filePath) ?? throw new FileBuilderException(filePath),
                            Path.GetDirectoryName(filePath) ?? throw new FileBuilderException(filePath),
                            SourceLineBuilder.BuildSourceLineFromLineOfCode(fileContent.ToArray()),
                            Option<FileGitStats>.None,
                            Option<FileCoverageStats>.None);
        }
    }
}
