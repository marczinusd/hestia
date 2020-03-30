using System.Diagnostics.CodeAnalysis;
using Hestia.ConsoleRunner.Configuration;
using Hestia.Model.Builders;
using Hestia.Model.Stats;
using Hestia.Model.Wrappers;
using Microsoft.Extensions.Logging;
using IOFile = System.IO.File;

namespace Hestia.ConsoleRunner
{
    // ReSharper disable once ClassNeverInstantiated.Global
    [ExcludeFromCodeCoverage]
    public class Program
    {
        public static void Main(string[] args)
        {
            var factory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });
            var executor = new CommandLineExecutor();
            var ioWrapper = new DiskIOWrapper();
            var reportConverter = new CoverageReportConverter(ioWrapper, new ReportGeneratorWrapper());
            var enricher = new StatsEnricher(ioWrapper,
                                             new GitCommands(executor),
                                             factory.CreateLogger<IStatsEnricher>(),
                                             executor,
                                             new CoverageProviderFactory(ioWrapper),
                                             new PathValidator(),
                                             reportConverter);
            var runner = new HestiaConsoleRunner(factory,
                                                 enricher,
                                                 new JsonConfigurationProvider(),
                                                 ioWrapper,
                                                 new PathValidator(),
                                                 reportConverter);

            runner.Run(args);
        }
    }
}
