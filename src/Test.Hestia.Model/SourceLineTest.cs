using FluentAssertions;
using Hestia.Model;
using Hestia.Model.Stats;
using Xunit;

namespace Test.Hestia.Model
{
    public class SourceLineTest
    {
        [Fact]
        public void WithShouldCreateNewObject()
        {
            var line = new SourceLine(1,
                                      "bla",
                                      new LineCoverageStats(true),
                                      new LineGitStats(1, 2, 3));

            line.Should()
                .NotBeSameAs(line.With());
        }

        [Fact]
        public void WithShouldCorrectlyOverridePropsWithProvidedValues()
        {
            var line = new SourceLine(1,
                                      "bla",
                                      new LineCoverageStats(false),
                                      new LineGitStats(1, 2, 3));

            var newLine = line.With(new LineCoverageStats(true), new LineGitStats(2, 3, 4));

            newLine.LineCoverageStats
                   .Match(x => x.IsCovered, () => false)
                   .Should()
                   .BeTrue();
            newLine.LineGitStats.Match(x => x.LineNumber, () => -1)
                   .Should()
                   .Be(2);
            newLine.LineGitStats.Match(x => x.ModifiedInNumberOfCommits, () => -1)
                   .Should()
                   .Be(3);
            newLine.LineGitStats.Match(x => x.NumberOfLifetimeAuthors, () => -1)
                   .Should()
                   .Be(4);
        }
    }
}
