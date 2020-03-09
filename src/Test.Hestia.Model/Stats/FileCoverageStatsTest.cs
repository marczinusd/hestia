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
            var stats = new FileCoverageStats(new FileCoverage("bla", new[] { (1, 0), (2, 0), (3, 0), (4, 1) }));

            stats.PercentageOfLineCoverage
                 .Should()
                 .BeApproximately(25m, 0.01m);
        }
    }
}
