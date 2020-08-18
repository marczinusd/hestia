using FluentAssertions;
using Hestia.Model.Builders;
using Xunit;

namespace Test.Hestia.Model.Builders
{
    public class SourceLineBuilderTest
    {
        private const string FirstLine = "first line";
        private const string SecondLine = "second line";

        [Fact]
        public void SourceLineBuilderShouldReturnEmptyArrayForNoInputs() =>
            SourceLineBuilder.BuildSourceLineFromLineOfCode(new string[0])
                             .Should()
                             .BeEmpty();

        [Fact]
        public void SourceLineBuilderShouldReturnTwoDistinctLinesForTwoLinesOfCode()
        {
            var result = SourceLineBuilder.BuildSourceLineFromLineOfCode(new[] { FirstLine, SecondLine });

            result[0]
                .Text.Should()
                .Be(FirstLine);
            result[0]
                .LineNumber.Should()
                .Be(1);

            result[1]
                .Text.Should()
                .Be(SecondLine);
            result[1]
                .LineNumber.Should()
                .Be(2);
        }
    }
}
