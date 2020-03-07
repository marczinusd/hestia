using System.Diagnostics.CodeAnalysis;
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
            var enricher = new StatsEnricher(new DiskIOWrapper(),
                                             new GitCommands(executor),
                                             factory.CreateLogger<IStatsEnricher>(),
                                             executor);
            var runner = new HestiaConsoleRunner(factory,
                                                 enricher);

            runner.Run(args);
        }
    }
}
