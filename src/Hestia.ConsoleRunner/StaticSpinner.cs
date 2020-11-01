using System;
using System.Diagnostics.CodeAnalysis;

namespace Hestia.ConsoleRunner
{
    [ExcludeFromCodeCoverage]
    public class StaticSpinner : ISpinner
    {
        public void Start(string message, Action actionToRun)
        {
            var initialColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("    " + message);
            try
            {
                actionToRun();
                Console.Write("\r✅   " + message);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(" [SUCCESS]\n");
                Console.ForegroundColor = initialColor;
            }
            catch (Exception)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("\r⛔   ");
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write(message);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(" [FAILURE]\n");
                Console.ForegroundColor = initialColor;
                throw;
            }
            finally
            {
                Console.ForegroundColor = initialColor;
            }
        }
    }
}
