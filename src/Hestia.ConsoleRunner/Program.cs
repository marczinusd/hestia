using System.Text.Json;
using CommandLine;
using Hestia.Model;
using Hestia.Model.Stats;
using Hestia.Model.Wrappers;
using static LanguageExt.Prelude;
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
                      var enricher = new StatsEnricher(new DiskIOWrapper(), new GitCommands());
                      var rootDirectory = DirectoryBuilder.BuildDirectoryStructureFromFilePath(options.RepositoryPath);
                      var repository = new Repository(options.RepositoryId,
                                                      options.RepositoryName,
                                                      rootDirectory,
                                                      Some(options.CoveragePath));
                      var enrichedRepository = enricher.Enrich(repository);

                      IOFile.WriteAllText(options.OutputPath, JsonSerializer.Serialize(enrichedRepository));
                  });
        }

        private class Options
        {
            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            [Option('r',
                    "repositoryPath",
                    Required = true,
                    HelpText = "Path to the repository to analyze")]
            public string RepositoryPath { get; set; }

            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            [Option('c',
                    "coveragePath",
                    Required = false,
                    HelpText = "Path to the coverage result file for the selected repo")]
            public string CoveragePath { get; set; }

            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            [Option('i',
                    "repositoryId",
                    Required = false,
                    Default = 0,
                    HelpText =
                        "Used to specify the id of the repository which will appear in the JSON representation.")]
            public int RepositoryId { get; set; }

            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            [Option('n',
                    "repositoryName",
                    Required = false,
                    Default = "dummy",
                    HelpText = "Used to specify the name of the repository that'll appear in the json representation.")]
            public string RepositoryName { get; set; }

            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            [Option('o',
                    "outpath",
                    Required = true,
                    HelpText = "Path to write the repository's JSON representation to",
                    Default = "repository.json")]
            public string OutputPath { get; set; }
        }
    }
}
