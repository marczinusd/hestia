using System;
using FluentAssertions;
using Hestia.Model;
using LanguageExt;
using Xunit;

namespace Test.Hestia.Model
{
    public class RepositoryTest
    {
        [Fact]
        public void WithShouldReturnNewObject()
        {
            var repo = new Repository(1,
                                      "bla",
                                      Array.Empty<RepositorySnapshot>(),
                                      "someCommand",
                                      "lcov.info");

            repo.With()
                .Should()
                .NotBeSameAs(repo);
        }

        [Fact]
        public void WithShouldCorretlyOverridePropertiesWithProvidedValues()
        {
            var repo = new Repository(1,
                                      "bla",
                                      Option<RepositorySnapshot[]>.None,
                                      string.Empty,
                                      string.Empty);

            var newRepo = repo.With(Array.Empty<RepositorySnapshot>(), "bla", "lcov.info");

            newRepo.Snapshots
                   .Match(x => x, null as RepositorySnapshot[])
                   .Should()
                   .BeEmpty();
            newRepo.CoverageExecutionCommand
                   .Match(x => x, string.Empty)
                   .Should()
                   .Be("bla");
            newRepo.CoverageOutputLocation
                   .Match(x => x, string.Empty)
                   .Should()
                   .Be("lcov.info");
        }
    }
}
