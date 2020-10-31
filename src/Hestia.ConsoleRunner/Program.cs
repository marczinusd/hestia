using Serilog;

namespace Hestia.ConsoleRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                         .MinimumLevel.Debug()
                         .WriteTo.Console()
                         .CreateLogger();
            var log = Log.Logger;

            log.Information("Hello World!");
        }
    }
}
