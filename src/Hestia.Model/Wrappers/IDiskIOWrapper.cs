namespace Hestia.Model.Wrappers
{
    public interface IDiskIOWrapper
    {
        SourceLine[] ReadAllLinesFromFile(string filePath);
    }
}
