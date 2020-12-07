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
        private readonly IXmlFileSerializationWrapper _xmlFileSerializationWrapper;

        public CoverageProviderFactory(IDiskIOWrapper ioWrapper,
                                       IXmlFileSerializationWrapper xmlFileSerializationWrapper)
        {
            _ioWrapper = ioWrapper;
            _xmlFileSerializationWrapper = xmlFileSerializationWrapper;
        }

        public ICoverageProvider CreateProviderForFile(string filePath)
        {
            if (Path.GetFileName(filePath)
                    .Contains("coverage.json", StringComparison.OrdinalIgnoreCase))
            {
                return new JsonCovCoverageProvider(_ioWrapper);
            }

            if (Path.GetFileName(filePath)
                    .Contains("cobertura", StringComparison.OrdinalIgnoreCase))
            {
                return new CoberturaCoverageProvider(_xmlFileSerializationWrapper);
            }

            throw new InvalidOperationException($"Coverage report at {filePath} is not supported");
        }
    }
}
