using System.Collections.Generic;
using FluentAssertions;
using Hestia.Model;
using Hestia.Model.Stats;
using LanguageExt;
using Xunit;

namespace Test.Hestia.Model
{
    public class DirectoryTest
    {
        [Fact]
        public void WithShouldReturnNewDirectoryObject()
        {
            var directory = new Directory(string.Empty,
                                          string.Empty,
                                          new List<Directory>(),
                                          new List<File>());

            directory.With()
                     .Should()
                     .NotBeSameAs(directory);
        }

        [Fact]
        public void WithShouldOverridePropertiesWithProvidedValuesTest()
        {
            var directory = new Directory(string.Empty,
                                          string.Empty,
                                          new List<Directory>(),
                                          new List<File>());

            var newDirectory = directory.With(new List<File>
                                              {
                                                  new File(string.Empty,
                                                           string.Empty,
                                                           string.Empty,
                                                           new List<SourceLine>(),
                                                           Option<FileGitStats>.None,
                                                           Option<FileCoverageStats>.None),
                                              },
                                              new List<Directory>
                                              {
                                                  new Directory(string.Empty,
                                                                string.Empty,
                                                                new List<Directory>(),
                                                                new List<File>()),
                                              });

            newDirectory.Directories
                        .Should()
                        .HaveCount(1);
            newDirectory.Files
                        .Should()
                        .HaveCount(1);
        }
    }
}
