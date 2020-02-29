using System;
using System.Collections.Generic;
using Hestia.Model.Wrappers;

namespace Hestia.Model.Stats
{
    public static class CoverageProviders
    {
        public static IDictionary<string, Func<IDiskIOWrapper, ICoverageProvider>> Providers { get; } =
            new Dictionary<string, Func<IDiskIOWrapper, ICoverageProvider>>
            {
                { nameof(JsonCovCoverageProvider), wrapper => new JsonCovCoverageProvider(wrapper) },
            };
    }
}
