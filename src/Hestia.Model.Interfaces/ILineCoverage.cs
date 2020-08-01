namespace Hestia.Model.Interfaces
{
    public interface ILineCoverage
    {
        int LineNumber { get; }

        int HitCount { get; }
    }
}
