using FluentAssertions;
using Hestia.Model.Stats;
using Xunit;

namespace Test.Hestia.Model.Stats
{
    public class FileCoverageTest
    {
        [Fact]
        public void ToStringShouldCreateCorrectStringRepresentation()
        {
            var coverage = new FileCoverage("bla", new (int lineNumber, int hitCount)[] { (1, 2) });

            coverage.ToString()
                    .Should()
                    .Be("bla : (1, 2)");
        }
    }
}
