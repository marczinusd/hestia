using System.IO;
using System.Linq;
using Hestia.Model.Interfaces;
using Hestia.Model.Wrappers;
using LanguageExt;

namespace Hestia.Model.Builders
{
    public static class FileBuilder
    {
        public static IFile BuildFileFromPath(string filePath, IDiskIOWrapper diskIoWrapper)
        {
            var fileContent = diskIoWrapper.ReadAllLinesFromFile(filePath);

            return new File(Path.GetFileName(filePath) ?? throw new FileBuilderException(filePath),
                            Path.GetExtension(filePath)!,
                            Path.GetDirectoryName(filePath)!,
                            SourceLineBuilder.BuildSourceLineFromLineOfCode(fileContent.ToArray()),
                            Option<IFileGitStats>.None,
                            Option<IFileCoverageStats>.None);
        }
    }
}
