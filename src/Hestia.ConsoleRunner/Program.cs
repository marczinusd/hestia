using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using Autofac;
using Hestia.DAL.EFCore;
using Hestia.DAL.Interfaces;
using Hestia.Model.Builders;
using Hestia.Model.Stats;
using Hestia.Model.Wrappers;
using NJsonSchema;
using Serilog;

namespace Hestia.ConsoleRunner
{
    [ExcludeFromCodeCoverage]
    internal static class Program
    {
        private static readonly string ConfigSchemaJsonLocation =
            Path.Join(Path.GetDirectoryName(Assembly.GetExecutingAssembly()
                                                    .Location),
                      "config.schema.json");

        public static int Main(string[] args)
        {
            var rootCommand = new RootCommand
            {
                new Option("--configPath", "Path to configuration file") { Argument = new Argument<string>() },
                new Option("--debug", "Set log level to debug"),
                new Option("--dryRun", "Skip committing to database -- useful for testing")
            };
            rootCommand.Description = "Console runner for Hestia analysis";
            rootCommand.Handler = CommandHandler.Create<string, bool, bool>((configPath, debug, dryRun) =>
            {
                var log = CreateLogger(debug);
                log.Information($"Running with config at {configPath}");
                try
                {
                    if (!File.Exists(configPath))
                    {
                        log.Error($"Config file not found at path: {configPath}");
                        log.Error("Stopping execution...");
                        return;
                    }

                    var configAsText = File.ReadAllText(configPath);

                    if (!ValidateJsonConfig(log, configAsText))
                    {
                        return;
                    }

                    var config = JsonSerializer.Deserialize<RunnerConfig>(configAsText)!;
                    var container = SetupIOC(config);
                    var runner = container.Resolve<Runner>();

                    runner.BuildFromConfig(config, dryRun);
                    log.Information("Done!");
                }
                catch (Exception e)
                {
                    log.Error($"A fatal error occured during processing: {e.Message}. Exiting...");
                }
            });

            return rootCommand.InvokeAsync(args)
                              .Result;
        }

        private static bool ValidateJsonConfig(ILogger log, string configAsText)
        {
            if (!File.Exists(ConfigSchemaJsonLocation))
            {
                log.Warning("JSON Schema not found -- skipping config validation.");
                return true;
            }

            var result = JsonSchema.FromFileAsync(ConfigSchemaJsonLocation)
                                   .Result.Validate(configAsText);
            if (!result.Any())
            {
                log.Information("Provided config passed validation without errors.");
                return true;
            }

            log.Error("Validation of config failed against schema:");
            foreach (var error in result)
            {
                log.Error(error.ToString());
            }

            return false;
        }

        private static ILogger CreateLogger(bool enableDebug)
        {
            var config = new LoggerConfiguration();
            config = enableDebug ? config.MinimumLevel.Debug() : config.MinimumLevel.Information();

            Log.Logger = config.WriteTo.Console()
                               .CreateLogger();

            return Log.Logger;
        }

        private static IContainer SetupIOC(RunnerConfig config)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<DiskIOWrapper>()
                   .As<IDiskIOWrapper>();
            builder.RegisterType<GitCommands>()
                   .As<IGitCommands>();
            builder.RegisterType<CommandLineExecutor>()
                   .As<ICommandLineExecutor>()
                   .WithParameter("echoMode", ExecutorEchoMode.NoEcho);
            builder.RegisterType<CoverageProviderFactory>()
                   .As<ICoverageProviderFactory>();
            builder.RegisterType<PathValidator>()
                   .As<IPathValidator>();
            builder.RegisterType<ReportGeneratorWrapper>()
                   .As<IReportGeneratorWrapper>();
            builder.RegisterType<CoverageReportConverter>()
                   .As<ICoverageReportConverter>();
            builder.RegisterInstance(Log.Logger);
            builder.RegisterType<StatsEnricher>()
                   .As<IStatsEnricher>();
            builder.RegisterType<RepositorySnapshotBuilderWrapper>()
                   .As<IRepositorySnapshotBuilderWrapper>();
            builder.RegisterType<Runner>();

            builder.RegisterType<SnapshotEFClient>()
                   .As<ISnapshotPersistence>();
            builder.RegisterType<XmlFileSerializationWrapper>()
                   .As<IXmlFileSerializationWrapper>();
            builder.RegisterInstance(DbSetup.CreateContext(config.SqliteDbName, config.SqliteDbLocation));

            return builder.Build();
        }
    }
}
