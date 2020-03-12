using FluentAssertions;
using Hestia.Model.Stats;
using Xunit;

namespace Test.Hestia.Model.Stats
{
    public class LineCoverageTest
    {
        [Fact]
        public void LineCoverageImplementsCorrectEqualsMethod()
        {
            var lineCoverage1 = new LineCoverage(1, 1);
            var lineCoverage1Again = lineCoverage1;
            var lineCoverage2 = new LineCoverage(1, 1);

            lineCoverage1.Should()
                         .BeEquivalentTo(lineCoverage2);
            lineCoverage1.Equals(null)
                         .Should()
                         .BeFalse();
            lineCoverage1.Equals(lineCoverage1Again)
                         .Should()
                         .BeTrue();
            lineCoverage1.Should()
                         .NotBe("bla");
            lineCoverage1.GetHashCode()
                         .Should()
                         .Be(lineCoverage2.GetHashCode());
        }
    }
}
