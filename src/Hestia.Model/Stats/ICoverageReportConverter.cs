using LanguageExt;

namespace Hestia.Model.Stats
{
    public interface ICoverageReportConverter
    {
        /// <summary>
        ///     Converts the coverage report to coverage json format.
        /// </summary>
        /// <param name="inputFilePath">Path to the coverage report to be used as input.</param>
        /// <param name="outputLocation">Destination directory of the resulting coverage file.</param>
        /// <returns>Full path to the resulting coverage json file.</returns>
        Option<string> Convert(string inputFilePath, string outputLocation);
    }
}
