using System;
using FluentAssertions;
using Hestia.ConsoleRunner;
using Hestia.Model.Stats;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Test.Hestia.ConsoleRunner
{
    public class HestiaConsoleRunnerTest
    {
        [Fact]
        public void LoggerFactoryCannotBeNull()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Action act = () => new HestiaConsoleRunner(null, null, null);

            act.Should()
               .Throw<ArgumentNullException>()
               .WithMessage("*loggerFactory*");
        }

        [Fact]
        public void StatsEnricherCannotBeNull()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Action act = () => new HestiaConsoleRunner(new LoggerFactory(), null, null);

            act.Should()
               .Throw<ArgumentNullException>()
               .WithMessage("*statsEnricher*");
        }

        [Fact]
        public void JsonConfigProviderCannotBeNull()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Action act = () => new HestiaConsoleRunner(new LoggerFactory(), Mock.Of<IStatsEnricher>(), null);

            act.Should()
               .Throw<ArgumentNullException>()
               .WithMessage("*configurationProvider*");
        }
    }
}
