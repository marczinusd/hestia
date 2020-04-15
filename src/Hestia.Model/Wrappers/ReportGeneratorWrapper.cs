using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Palmmedia.ReportGenerator.Core;

namespace Hestia.Model.Wrappers
{
    [ExcludeFromCodeCoverage]
    public class ReportGeneratorWrapper : IReportGeneratorWrapper
    {
        public bool Generate(string inputFilePath, string outputLocation)
        {
            var builder = new ReportConfigurationBuilder();
            var config = builder.Create(new Dictionary<string, string>
            {
                { "Reports", $"{inputFilePath}" },
                { "TargetDir", $"{outputLocation}" },
                { "ReportTypes", "Cobertura" },
            });
            IReportGenerator generator = new Generator();

            return generator.GenerateReport(config);
        }
    }
}
