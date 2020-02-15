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
            StatsEnricher enricher = new StatsEnricher(new DiskIOWrapper());
            Console.WriteLine("Hello World!");
        }
    }
}
