﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using CommandLine;
using Hestia.Model;
using Hestia.Model.Builders;
using Hestia.Model.Stats;
using Hestia.Model.Wrappers;
using Microsoft.Extensions.Logging;
using IOFile = System.IO.File;

namespace Hestia.ConsoleRunner
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Program
    {
        public static void Main(string[] args)
        {
            Parser.Default
                  .ParseArguments<Options>(args)
                  .WithParsed(options =>
                  {
                      var factory = LoggerFactory.Create(builder =>
                      {
                          builder.AddConsole();
                      });
                      var logger = factory.CreateLogger<Program>();
                      var enricher = new StatsEnricher(new DiskIOWrapper(),
                                                       new GitCommands(new CommandLineExecutor()),
                                                       factory.CreateLogger<StatsEnricher>());
                      var repository = BuildRepositoryWithOptions(options);

                      var enrichedRepository = enricher.EnrichWithCoverage(enricher.EnrichWithGitStats(repository));

                      logger.LogInformation($"Writing results to output...");
                      IOFile.WriteAllText(options.OutputPath,
                                          JsonSerializer.Serialize(enrichedRepository,
                                                                   new JsonSerializerOptions
                                                                   {
                                                                       WriteIndented = true,
                                                                   }));
                      logger.LogInformation($"Output created at {options.OutputPath}");
                  });
        }

        private static Repository BuildRepositoryWithOptions(Options options) =>
            RepositoryBuilder.BuildRepositoryFromDirectoryPath(options.RepositoryId,
                                                               options.RepositoryName,
                                                               options.RepositoryPath,
                                                               Path.Join(options
                                                                             .RepositoryPath,
                                                                         options.SourcePath),
                                                               options.SourceExtensions
                                                                      .ToArray(),
                                                               options.CoveragePath,
                                                               new DiskIOWrapper(),
                                                               new PathValidator());

        // ReSharper disable once ClassNeverInstantiated.Local
        private class Options
        {
            public Options(string repositoryPath,
                           string coveragePath,
                           int repositoryId,
                           string repositoryName,
                           string outputPath,
                           IEnumerable<string> sourceExtensions,
                           string sourcePath)
            {
                RepositoryPath = repositoryPath;
                CoveragePath = coveragePath;
                RepositoryId = repositoryId;
                RepositoryName = repositoryName;
                OutputPath = outputPath;
                SourceExtensions = sourceExtensions;
                SourcePath = sourcePath;
            }

            [Option('r',
                    "repositoryPath",
                    Required = true,
                    HelpText = "Path to the repository to analyze")]
            public string RepositoryPath { get; }

            [Option('c',
                    "coveragePath",
                    Required = false,
                    HelpText = "Path to the coverage result file for the selected repo")]
            public string CoveragePath { get; }

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
            public string RepositoryName { get; }

            [Option('o',
                    "outPath",
                    Required = false,
                    HelpText = "Path to write the repository's JSON representation to",
                    Default = "repository.json")]
            public string OutputPath { get; }

            [Option('e',
                    "extensions",
                    Required = true,
                    HelpText =
                        "Array of valid source file extensions you want to parse, separated by a comma, e.g: '.js,.ts'",
                    Separator = ',')]
            public IEnumerable<string> SourceExtensions { get; }

            [Option('s',
                    "sourcePath",
                    Required = false,
                    HelpText = "Path **relative** to the rootpath, where the source code is located",
                    Default = "src")]
            public string SourcePath { get; }
        }
    }
}
