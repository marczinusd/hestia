using System;
using System.Collections.Generic;
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
using Serilog;
using File = System.IO.File;

namespace Hestia.ConsoleRunner
{
    public class HestiaConsoleRunner
    {
        private readonly ILogger _logger;
        private readonly IStatsEnricher _statsEnricher;
        private readonly IJsonConfigurationProvider _configurationProvider;
        private readonly IDiskIOWrapper _ioWrapper;
        private readonly IPathValidator _validator;
        private readonly ICoverageReportConverter _converter;

        public HestiaConsoleRunner(ILogger logger,
                                   IStatsEnricher statsEnricher,
                                   IJsonConfigurationProvider configurationProvider,
                                   IDiskIOWrapper ioWrapper,
                                   IPathValidator validator,
                                   ICoverageReportConverter converter)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _statsEnricher = statsEnricher ?? throw new ArgumentNullException(nameof(statsEnricher));
            _configurationProvider =
                configurationProvider ?? throw new ArgumentNullException(nameof(configurationProvider));
            _ioWrapper = ioWrapper;
            _validator = validator;
            _converter = converter;
        }

        public void Run(IEnumerable<string> args)
        {
            Parser.Default
                  .ParseArguments<Options>(args)
                  .WithParsed(Execute);
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

        private async void Execute(Options options)
        {
            var config = await _configurationProvider.LoadConfiguration(options.JsonConfigPath);

            // ReSharper disable once PossibleNullReferenceException
            if (!string.IsNullOrWhiteSpace(config.CoverageReportLocation) && !Path
                                                                              .GetFileName(config
                                                                                               .CoverageReportLocation)
                                                                              .ToLower()
                                                                              .Contains("coverage.json"))
            {
                // ReSharper disable once AssignNullToNotNullAttribute
                var result = _converter.Convert(config.CoverageReportLocation,
                                                Path.GetDirectoryName(config.CoverageReportLocation) ??
                                                throw new DirectoryNotFoundException(config.CoverageReportLocation))
                                       .Match(res => res, () => string.Empty);
                config = config.With(string.IsNullOrWhiteSpace(result) ? null : result);
            }

            var repository = BuildRepositoryWithOptions(options,
                                                        config,
                                                        _ioWrapper,
                                                        _validator);

            var enrichedRepository = repository.Apply(_statsEnricher.EnrichWithCoverage)
                                               .Apply(x => _statsEnricher.EnrichWithGitStats(x, GitStatGranularity.File));

            _logger.Information("Writing results to output...");
            File.WriteAllText(options.OutputPath,
                              JsonSerializer.Serialize(enrichedRepository,
                                                       new JsonSerializerOptions { WriteIndented = true, }));
            _logger.Information($"Output created at {options.OutputPath}");
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        [ExcludeFromCodeCoverage]
        private class Options
        {
            public Options(string repositoryId,
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
                    Default = "",
                    HelpText =
                        "Used to specify the id of the repository which will appear in the JSON representation")]
            public string RepositoryId { get; }

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
