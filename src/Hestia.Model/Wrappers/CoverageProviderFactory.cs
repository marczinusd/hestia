using Hestia.Model.Stats;

namespace Hestia.Model.Wrappers
{
    public class CoverageProviderFactory : ICoverageProviderFactory
    {
        private readonly IDiskIOWrapper _ioWrapper;

        public CoverageProviderFactory(IDiskIOWrapper ioWrapper)
        {
            _ioWrapper = ioWrapper;
        }

        public ICoverageProvider CreateProviderForFile() =>
            new JsonCovCoverageProvider(_ioWrapper);
    }
}
