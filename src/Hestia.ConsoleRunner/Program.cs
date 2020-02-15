using System;
using Hestia.Model.Stats;
using Hestia.Model.Wrappers;

namespace Hestia.ConsoleRunner
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // ReSharper disable once UnusedVariable
            var enricher = new StatsEnricher(new DiskIOWrapper(), new GitCommands());
            Console.WriteLine("Hello World!");
        }
    }
}
