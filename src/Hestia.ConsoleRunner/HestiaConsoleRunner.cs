using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text.Json;
using CommandLine;
using Hestia.ConsoleRunner.Configuration;
using Hestia.Model;
using Hestia.Model.Builders;
using Hestia.Model.Stats;
using Hestia.Model.Wrappers;
using LanguageExt;
using Microsoft.Extensions.Logging;
using File = System.IO.File;

namespace Hestia.ConsoleRunner
{
    public class HestiaConsoleRunner
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly IStatsEnricher _statsEnricher;
        private readonly IJsonConfigurationProvider _configurationProvider;
        private readonly IDiskIOWrapper _ioWrapper;
        private readonly IPathValidator _validator;

        public HestiaConsoleRunner(ILoggerFactory loggerFactory,
                                   IStatsEnricher statsEnricher,
                                   IJsonConfigurationProvider configurationProvider,
                                   IDiskIOWrapper ioWrapper,
                                   IPathValidator validator)
        {
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _statsEnricher = statsEnricher ?? throw new ArgumentNullException(nameof(statsEnricher));
            _configurationProvider =
                configurationProvider ?? throw new ArgumentNullException(nameof(configurationProvider));
            _ioWrapper = ioWrapper;
            _validator = validator;
        }

        public void Run(string[] args)
        {
            Parser.Default
                  .ParseArguments<Options>(args)
                  .WithParsed(options =>
                  {
                      var logger = _loggerFactory.CreateLogger<HestiaConsoleRunner>();
                      var config = _configurationProvider.LoadConfiguration(options.JsonConfigPath).Result;
                      var repository = BuildRepositoryWithOptions(options, config, _ioWrapper, _validator);

                      var enrichedRepository = repository.Apply(_statsEnricher.EnrichWithCoverage)
                                                         .Apply(_statsEnricher.EnrichWithGitStats);

                      logger.LogInformation("Writing results to output...");
                      File.WriteAllText(options.OutputPath,
                                        JsonSerializer.Serialize(enrichedRepository,
                                                                 new JsonSerializerOptions { WriteIndented = true, }));
                      logger.LogInformation($"Output created at {options.OutputPath}");
                  });
        }

        private static RepositorySnapshot BuildRepositoryWithOptions(Options options,
                                                                     ConsoleRunnerConfig config,
                                                                     IDiskIOWrapper ioWrapper,
                                                                     IPathValidator validator) =>
            new RepositorySnapshotBuilderArguments(options.RepositoryId,
                                                   config.RepoPath,
                                                   Path.Join(config.RepoPath,
                                                             config.SourceRoot),
                                                   config.SourceExtensions
                                                          .ToArray(),
                                                   config.CoverageOutputLocation,
                                                   Option<string>.None,
                                                   Option<DateTime>.None,
                                                   ioWrapper,
                                                   validator).Build();

        // ReSharper disable once ClassNeverInstantiated.Local
        [ExcludeFromCodeCoverage]
        private class Options
        {
            public Options(int repositoryId,
                           string repositoryName,
                           string outputPath,
                           string jsonConfigPath)
            {
                RepositoryId = repositoryId;
                RepositoryName = repositoryName;
                OutputPath = outputPath;
                JsonConfigPath = jsonConfigPath;
            }

            [Option('i',
                    "repositoryId",
                    Required = false,
                    Default = 0,
                    HelpText =
                        "Used to specify the id of the repository which will appear in the JSON representation")]
            public int RepositoryId { get; }

            [Option('n',
                    "repositoryName",
                    Required = false,
                    Default = "dummy",
                    HelpText =
                        "Used to specify the name of the repository that will appear in the json representation")]

            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public string RepositoryName { get; }

            [Option('o',
                    "outPath",
                    Required = false,
                    HelpText = "Path to write the repository's JSON representation to",
                    Default = "repository.json")]
            public string OutputPath { get; }

            [Option('j',
                    "jsonConfigPath",
                    Required = true,
                    HelpText = "Path to the json configuration file",
                    Default = "Repository.example.json")]
            public string JsonConfigPath { get; }
        }
    }
}
