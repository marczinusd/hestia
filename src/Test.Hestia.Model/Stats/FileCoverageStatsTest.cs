using System.Linq;
using FluentAssertions;
using Hestia.Model.Stats;
using Xunit;

namespace Test.Hestia.Model.Stats
{
    public class FileCoverageStatsTest
    {
        [Fact]
        public void FileCoverageStatsShouldCalculateLineCoverageCorrectly()
        {
            var stats = new FileCoverageStats(new FileCoverage("bla",
                                                               new[]
                                                               {
                                                                   (1, 0, true, "1/1"), (2, 0, true, "1/1"),
                                                                   (3, 0, true, "1/1"), (4, 1, true, "1/1")
                                                               }));

            stats.PercentageOfLineCoverage
                 .Should()
                 .BeApproximately(25m, 0.01m);
        }

        [Fact]
        public void CoverageShouldBeZeroIfNoStatsArePresent()
        {
            var stats = new FileCoverageStats(new FileCoverage("bla",
                                                               Enumerable
                                                                   .Empty<(int lineNumber, int hitCount, bool branch,
                                                                       string conditionCoverage)>()));

            stats.PercentageOfLineCoverage
                 .Should()
                 .Be(0);
        }
    }
}
