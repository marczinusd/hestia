namespace Hestia.Model.Wrappers
{
    public interface IReportGeneratorWrapper
    {
        bool Generate(string inputFilePath, string outputLocation);
    }
}
