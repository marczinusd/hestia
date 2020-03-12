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
            var enricher = new StatsEnricher(ioWrapper,
                                             new GitCommands(executor),
                                             factory.CreateLogger<IStatsEnricher>(),
                                             executor,
                                             new CoverageProviderFactory(ioWrapper),
                                             new PathValidator());
            var runner = new HestiaConsoleRunner(factory,
                                                 enricher,
                                                 new JsonConfigurationProvider());

            runner.Run(args);
        }
    }
}
