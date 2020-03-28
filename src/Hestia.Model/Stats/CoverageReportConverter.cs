using System.IO;
using Hestia.Model.Wrappers;
using LanguageExt;

namespace Hestia.Model.Stats
{
    public class CoverageReportConverter : ICoverageReportConverter
    {
        private readonly IDiskIOWrapper _ioWrapper;
        private readonly IReportGeneratorWrapper _reportGeneratorWrapper;

        public CoverageReportConverter(IDiskIOWrapper ioWrapper, IReportGeneratorWrapper reportGeneratorWrapper)
        {
            _ioWrapper = ioWrapper;
            _reportGeneratorWrapper = reportGeneratorWrapper;
        }

        public Option<string> Convert(string inputFilePath, string outputLocation)
        {
            if (!_ioWrapper.FileExists(inputFilePath))
            {
                throw new
                    FileNotFoundException($"Input file at {inputFilePath} for coverage report conversion was not found.",
                                          inputFilePath);
            }

            return _reportGeneratorWrapper.Generate(inputFilePath, outputLocation) ? Path.Join(outputLocation, "coverage.json") : Option<string>.None;
        }
    }
}
