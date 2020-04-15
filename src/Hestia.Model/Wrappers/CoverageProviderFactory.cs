using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Hestia.Model.Cobertura;
using Hestia.Model.Stats;

namespace Hestia.Model.Wrappers
{
    [ExcludeFromCodeCoverage]
    public class CoverageProviderFactory : ICoverageProviderFactory
    {
        private readonly IDiskIOWrapper _ioWrapper;

        public CoverageProviderFactory(IDiskIOWrapper ioWrapper)
        {
            _ioWrapper = ioWrapper;
        }

        public ICoverageProvider CreateProviderForFile(string filePath)
        {
            if (Path.GetFileName(filePath).Contains("coverage.json", StringComparison.OrdinalIgnoreCase))
            {
                return new JsonCovCoverageProvider(_ioWrapper);
            }

            if (Path.GetFileName(filePath).Contains("cobertura.xml", StringComparison.OrdinalIgnoreCase))
            {
                return new CoberturaCoverageProvider();
            }

            throw new InvalidOperationException($"Coverage report at {filePath} is not supported");
        }
    }
}
