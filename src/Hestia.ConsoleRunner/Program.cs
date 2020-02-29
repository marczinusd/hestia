using System.Text.Json;
using CommandLine;
using Hestia.Model.Builders;
using Hestia.Model.Stats;
using Hestia.Model.Wrappers;
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
                      var ioWrapper = new DiskIOWrapper();
                      var enricher = new StatsEnricher(ioWrapper, new GitCommands(new CommandLineExecutor()));
                      var repository = RepositoryBuilder.BuildRepositoryFromDirectoryPath(-1,
                                                                                          options.RepositoryPath,
                                                                                          ioWrapper,
                                                                                          new PathValidator());
                      var enrichedRepository = enricher.EnrichWithGitStats(repository);

                      IOFile.WriteAllText(options.OutputPath, JsonSerializer.Serialize(enrichedRepository));
                  });
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        private class Options
        {
            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            [Option('r',
                    "repositoryPath",
                    Required = true,
                    HelpText = "Path to the repository to analyze")]
            public string RepositoryPath { get; set; }

            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            // ReSharper disable once UnusedMember.Local
            [Option('c',
                    "coveragePath",
                    Required = false,
                    HelpText = "Path to the coverage result file for the selected repo")]
            public string CoveragePath { get; set; }

            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            // ReSharper disable once UnusedMember.Local
            [Option('i',
                    "repositoryId",
                    Required = false,
                    Default = 0,
                    HelpText =
                        "Used to specify the id of the repository which will appear in the JSON representation.")]
            public int RepositoryId { get; set; }

            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            // ReSharper disable once UnusedMember.Local
            [Option('n',
                    "repositoryName",
                    Required = false,
                    Default = "dummy",
                    HelpText = "Used to specify the name of the repository that will appear in the json representation.")]
            public string RepositoryName { get; set; }

            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            [Option('o',
                    "outPath",
                    Required = true,
                    HelpText = "Path to write the repository's JSON representation to",
                    Default = "repository.json")]
            public string OutputPath { get; set; }
        }
    }
}
