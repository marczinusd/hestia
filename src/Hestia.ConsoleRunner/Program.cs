﻿using System.Diagnostics.CodeAnalysis;
using Hestia.ConsoleRunner.Configuration;
using Hestia.Model.Builders;
using Hestia.Model.Stats;
using Hestia.Model.Wrappers;
using Serilog;
using IOFile = System.IO.File;

namespace Hestia.ConsoleRunner
{
    // ReSharper disable once ClassNeverInstantiated.Global
    [ExcludeFromCodeCoverage]
    public static class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration().WriteTo
                                                  .Console()
                                                  .CreateLogger();
            var executor = new CommandLineExecutor(ExecutorEchoMode.NoEcho);
            var ioWrapper = new DiskIOWrapper();
            var reportConverter = new CoverageReportConverter(ioWrapper, new ReportGeneratorWrapper());
            var fileStreamWrapper = new XmlFileSerializationWrapper();
            var enricher = new StatsEnricher(ioWrapper,
                                             new GitCommands(executor),
                                             Log.Logger,
                                             executor,
                                             new CoverageProviderFactory(ioWrapper, fileStreamWrapper),
                                             new PathValidator(),
                                             reportConverter);
            var runner = new HestiaConsoleRunner(Log.Logger,
                                                 enricher,
                                                 new JsonConfigurationProvider(),
                                                 ioWrapper,
                                                 new PathValidator(),
                                                 reportConverter);

            runner.Run(args);
        }
    }
}
