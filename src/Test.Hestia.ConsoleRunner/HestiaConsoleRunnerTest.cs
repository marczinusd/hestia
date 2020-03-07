using System;
using FluentAssertions;
using Hestia.ConsoleRunner;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Test.Hestia.ConsoleRunner
{
    public class HestiaConsoleRunnerTest
    {
        [Fact]
        public void LoggerFactoryCannotBeNull()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Action act = () => new HestiaConsoleRunner(null, null);

            act.Should()
               .Throw<ArgumentNullException>()
               .WithMessage("*loggerFactory*");
        }

        [Fact]
        public void StatsEnricherCannotBeNull()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Action act = () => new HestiaConsoleRunner(new LoggerFactory(), null);

            act.Should()
               .Throw<ArgumentNullException>()
               .WithMessage("*statsEnricher*");
        }
    }
}
