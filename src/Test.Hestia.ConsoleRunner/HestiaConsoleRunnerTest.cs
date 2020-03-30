using System;
using AutoFixture;
using AutoFixture.AutoMoq;
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
            Action act = () => new HestiaConsoleRunner(null,
                                                       null,
                                                       null,
                                                       null,
                                                       null,
                                                       null);

            act.Should()
               .Throw<ArgumentNullException>()
               .WithMessage("*loggerFactory*");
        }

        [Fact]
        public void StatsEnricherCannotBeNull()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Action act = () => new HestiaConsoleRunner(new LoggerFactory(),
                                                       null,
                                                       null,
                                                       null,
                                                       null,
                                                       null);

            act.Should()
               .Throw<ArgumentNullException>()
               .WithMessage("*statsEnricher*");
        }

        [Fact]
        public void JsonConfigProviderCannotBeNull()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Action act = () => new HestiaConsoleRunner(new LoggerFactory(),
                                                       Mock.Of<IStatsEnricher>(),
                                                       null,
                                                       null,
                                                       null,
                                                       null);

            act.Should()
               .Throw<ArgumentNullException>()
               .WithMessage("*configurationProvider*");
        }

        // TODO: implement once the underlying code actually reads complete repos
        [Fact]
        public void ConsoleRunnerShouldExecuteStepsWhenGivenValidArguments()
        {
            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization { ConfigureMembers = true });

            var consoleRunner = fixture.Create<HestiaConsoleRunner>();

            consoleRunner.Run(new[] { "-j", "config.json" });
        }
    }
}
