namespace Hestia.Model.Wrappers
{
    public interface IGitCommands
    {
        long NumberOfChangesForFile(string filePath);
    }
}
