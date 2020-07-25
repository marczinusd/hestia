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
        private readonly IFileStreamWrapper _fileStreamWrapper;

        public CoverageProviderFactory(IDiskIOWrapper ioWrapper, IFileStreamWrapper fileStreamWrapper)
        {
            _ioWrapper = ioWrapper;
            _fileStreamWrapper = fileStreamWrapper;
        }

        public ICoverageProvider CreateProviderForFile(string filePath)
        {
            if (Path.GetFileName(filePath).Contains("coverage.json", StringComparison.OrdinalIgnoreCase))
            {
                return new JsonCovCoverageProvider(_ioWrapper);
            }

            if (Path.GetFileName(filePath).Contains("cobertura.xml", StringComparison.OrdinalIgnoreCase))
            {
                return new CoberturaCoverageProvider(_fileStreamWrapper);
            }

            throw new InvalidOperationException($"Coverage report at {filePath} is not supported");
        }
    }
}
