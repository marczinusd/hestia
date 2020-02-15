using System.Linq;
using CommandLine;
using Hestia.Model;
using Hestia.Model.Stats;
using Hestia.Model.Wrappers;
using static LanguageExt.Prelude;

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
                      enricher.Enrich(new Repository(0,
                                                     "dummy",
                                                     new Directory("a",
                                                                   options.RepositoryPath,
                                                                   Enumerable.Empty<Directory>(),
                                                                   Enumerable.Empty<File>()),
                                                     Some(options.CoveragePath)));
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

            // ReSharper disable once UnusedMember.Local
            [Option('o',
                    "outpath",
                    Required = true,
                    HelpText = "Path to write the repository's JSON representation",
                    Default = "repository.json")]
            public string OutputPath { get; set; }
        }
    }
}
