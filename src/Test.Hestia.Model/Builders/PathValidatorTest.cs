using System;
using System.IO;
using FluentAssertions;
using Hestia.Model.Builders;
using Xunit;

namespace Test.Hestia.Model.Builders
{
    public class PathValidatorTest
    {
        [Fact]
        public void ValidatorShouldThrowInvalidArgumentExceptionForNullParameter()
        {
            var validator = new PathValidator();

            Action act = () => validator.ValidateDirectoryPath(default);

            act.Should()
               .Throw<ArgumentException>();
        }

        [Fact]
        public void ValidatorShouldThrowInvalidArgumentExceptionForOnlyWhitespaceParameter()
        {
            var validator = new PathValidator();

            Action act = () => validator.ValidateDirectoryPath("     ");

            act.Should()
               .Throw<ArgumentException>();
        }

        [Fact]
        public void ValidatorShouldThrowInvalidOperationExceptionIfDirectoryProvidedDoesNotExist()
        {
            var validator = new PathValidator();

            Action act = () => validator.ValidateDirectoryPath("/bla/bla");

            act.Should()
               .Throw<InvalidOperationException>();
        }

        [Fact]
        public void ValidatorShouldNotThrowExceptionWhenGivenADirectoryThatExists() =>
            new PathValidator().ValidateDirectoryPath(Directory.GetCurrentDirectory());
    }
}
