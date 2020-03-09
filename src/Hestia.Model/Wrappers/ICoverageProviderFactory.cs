using Hestia.Model.Stats;

namespace Hestia.Model.Wrappers
{
    public interface ICoverageProviderFactory
    {
        ICoverageProvider CreateProviderForFile();
    }
}
