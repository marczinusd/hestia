using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Hestia.Model;
using Hestia.Model.Builders;
using Hestia.Model.Stats;
using Hestia.Model.Wrappers;
using Hestia.UIRunner.Services;
using Hestia.UIRunner.ViewModels;
using LanguageExt;
using Microsoft.Reactive.Testing;
using Moq;
using Test.Hestia.Utils;
using Test.Hestia.Utils.TestData;
using Xunit;
using File = Hestia.Model.File;

namespace Test.Hestia.UIRunner
{
    // Note: some tests require a very small wait due to some timing inconsistencies with ReactiveUI's validation specifically when running in CI
    public class FormViewModelTest
    {
        private const string RepoPath = "somePath";
        private const string SourceRoot = "sourceRoot";
        private const string CoverageCommand = "coverageCommand";
        private const string SourceExtensions = ".cs";
        private const int WaitMs = 25;

        private static readonly string CoverageOutputLocation = MockRepo.CreateFile()
                                                                        .FullPath;

        public static TheoryData<string> EmptyInputData => new TheoryData<string> { string.Empty, null, "     " };

        [Fact]
        public void RepositoryPathShouldPointToDirectoryThatExists()
        {
            var ioMock = new Mock<IDiskIOWrapper>();
            ioMock.Setup(mock => mock.DirectoryExists(RepoPath))
                  .Returns(false);

            var vm = new FormViewModel(ioMock.Object,
                                       Mock.Of<IStatsEnricher>(),
                                       Mock.Of<IPathValidator>(),
                                       Mock.Of<IRepositorySnapshotBuilderWrapper>(),
                                       Mock.Of<IOpenFileDialogService>(),
                                       Mock.Of<ICoverageReportConverter>()) { RepositoryPath = RepoPath, };
            Helpers.After(TimeSpan.FromMilliseconds(WaitMs),
                          () => vm.ValidationContext.Text
                                  .First()
                                  .Should()
                                  .Contain("Directory does not exist"));
        }

        [Fact]
        public void RepositoryPathShouldPointToGitRepository()
        {
            var ioMock = new Mock<IDiskIOWrapper>();
            ioMock.Setup(mock => mock.DirectoryExists(RepoPath))
                  .Returns(true);
            ioMock.Setup(mock => mock.DirectoryExists(Path.Join(RepoPath, ".git")))
                  .Returns(false);

            var vm = new FormViewModel(ioMock.Object,
                                       Mock.Of<IStatsEnricher>(),
                                       Mock.Of<IPathValidator>(),
                                       Mock.Of<IRepositorySnapshotBuilderWrapper>(),
                                       Mock.Of<IOpenFileDialogService>(),
                                       Mock.Of<ICoverageReportConverter>()) { RepositoryPath = RepoPath, };
            Helpers.After(TimeSpan.FromMilliseconds(WaitMs),
                          () => vm.ValidationContext.Text
                                  .First()
                                  .Should()
                                  .Contain("Directory is not a git repository"));
        }

        [Theory]
        [MemberData(nameof(EmptyInputData))]
        public void RepositoryPathEmptyFieldValidation(string input)
        {
            var vm = new FormViewModel(new DiskIOWrapper(),
                                       Mock.Of<IStatsEnricher>(),
                                       Mock.Of<IPathValidator>(),
                                       Mock.Of<IRepositorySnapshotBuilderWrapper>(),
                                       Mock.Of<IOpenFileDialogService>(),
                                       Mock.Of<ICoverageReportConverter>())
            {
                CoverageCommand = "bla",
                SourceExtensions = "bla",
                CoverageOutputLocation = "bla",
                RepositoryPath = input,
            };
            Helpers.After(TimeSpan.FromMilliseconds(WaitMs),
                          () => vm.ValidationContext.Text
                                  .Any(s => s.Contains("RepositoryPath should not be empty"))
                                  .Should()
                                  .BeTrue());
        }

        [Theory]
        [MemberData(nameof(EmptyInputData))]
        public void CoverageOutputLocationEmptyFieldValidation(string input)
        {
            var vm = new FormViewModel(new DiskIOWrapper(),
                                       Mock.Of<IStatsEnricher>(),
                                       Mock.Of<IPathValidator>(),
                                       Mock.Of<IRepositorySnapshotBuilderWrapper>(),
                                       Mock.Of<IOpenFileDialogService>(),
                                       Mock.Of<ICoverageReportConverter>())
            {
                RepositoryPath = "bla",
                CoverageCommand = "bla",
                SourceExtensions = "bla",
                CoverageOutputLocation = input,
            };
            Helpers.After(TimeSpan.FromMilliseconds(WaitMs),
                          () => vm.ValidationContext.Text
                                  .Any(s => s.Contains("CoverageOutputLocation should not be empty"))
                                  .Should()
                                  .BeTrue());
        }

        [Theory]
        [MemberData(nameof(EmptyInputData))]
        public void SourceExtensionsEmptyFieldValidation(string input)
        {
            var vm = new FormViewModel(new DiskIOWrapper(),
                                       Mock.Of<IStatsEnricher>(),
                                       Mock.Of<IPathValidator>(),
                                       Mock.Of<IRepositorySnapshotBuilderWrapper>(),
                                       Mock.Of<IOpenFileDialogService>(),
                                       Mock.Of<ICoverageReportConverter>())
            {
                RepositoryPath = "bla",
                CoverageCommand = "bla",
                CoverageOutputLocation = "bla",
                SourceExtensions = input,
            };

            Helpers.After(TimeSpan.FromMilliseconds(WaitMs),
                          () => vm.ValidationContext.Text
                                  .Any(s => s.Contains("SourceExtensions should not be empty"))
                                  .Should()
                                  .BeTrue());
        }

        [Fact(Skip = "Unreliable, fix later")]
        public void PressingProcessButtonInvokesSnapshotBuilderWithExpectedArguments()
        {
            var scheduler = new TestScheduler();
            var builderMock = new Mock<IRepositorySnapshotBuilderWrapper>();
            var converterMock = new Mock<ICoverageReportConverter>();
            converterMock.Setup(mock => mock.Convert(It.IsAny<string>(), It.IsAny<string>()))
                         .Returns("Cobertura.xml");
            var vm = new FormViewModel(Mock.Of<IDiskIOWrapper>(),
                                       Mock.Of<IStatsEnricher>(),
                                       Mock.Of<IPathValidator>(),
                                       builderMock.Object,
                                       Mock.Of<IOpenFileDialogService>(),
                                       converterMock.Object)
            {
                RepositoryPath = RepoPath,
                CoverageCommand = CoverageCommand,
                SourceExtensions = SourceExtensions,
                SourceRoot = SourceRoot,
                CoverageOutputLocation = CoverageOutputLocation,
            };

            scheduler.Start(() => vm.ProcessRepositoryCommand
                                    .Execute());

            Helpers.After(TimeSpan.FromMilliseconds(WaitMs),
                          () =>
                          {
                              builderMock.Verify(mock =>
                                                     mock.Build(It.Is<RepositorySnapshotBuilderArguments>(x =>
                                                                                                              MatchingRepositoryBuilderArgs(x))),
                                                 Times.Once);
                          });
        }

        [Fact]
        public void PressingProcessButtonShouldInvokeStatsEnricherWithExpectedRepositorySnapshot()
        {
            var scheduler = new TestScheduler();
            var statsEnricherMock = new Mock<IStatsEnricher>();
            var builderMock = new Mock<IRepositorySnapshotBuilderWrapper>();
            var converterMock = new Mock<ICoverageReportConverter>();
            var repositorySnapshot = new RepositorySnapshot(-1,
                                                            new List<File>(),
                                                            Option<string>.None,
                                                            Option<string>.None,
                                                            Option<DateTime>.None);
            converterMock.Setup(mock => mock.Convert(It.IsAny<string>(), It.IsAny<string>()))
                         .Returns(CoverageOutputLocation);
            builderMock.Setup(mock => mock.Build(It.IsAny<RepositorySnapshotBuilderArguments>()))
                       .Returns(repositorySnapshot);
            var vm = new FormViewModel(Mock.Of<IDiskIOWrapper>(),
                                       statsEnricherMock.Object,
                                       Mock.Of<IPathValidator>(),
                                       builderMock.Object,
                                       Mock.Of<IOpenFileDialogService>(),
                                       converterMock.Object)
            {
                RepositoryPath = "bla",
                CoverageCommand = "bla",
                SourceExtensions = "bla",
                SourceRoot = "src",
                CoverageOutputLocation = CoverageOutputLocation,
            };

            scheduler.Start(() => vm.ProcessRepositoryCommand
                                    .Execute());

            Helpers.After(TimeSpan.FromMilliseconds(WaitMs),
                          () =>
                          {
                              converterMock.Verify(mock => mock.Convert(CoverageOutputLocation,
                                                                        Path.GetDirectoryName(CoverageOutputLocation)),
                                                   Times.Once);
                              statsEnricherMock.Verify(mock => mock.EnrichWithCoverage(It.IsAny<RepositorySnapshot>()),
                                                       Times.Once);
                              statsEnricherMock.Verify(mock => mock.EnrichWithGitStats(It.IsAny<RepositorySnapshot>()),
                                                       Times.Once);
                          });
        }

        [Fact]
        public void ExecutingOpenFileDialogCommandInvokesFileDialogServiceAndPublishesResultOnRepositoryPath()
        {
            var scheduler = new TestScheduler();
            var fileDialogServiceMock = new Mock<IOpenFileDialogService>();
            fileDialogServiceMock.Setup(mock => mock.OpenFolderDialog())
                                 .Returns(Task.FromResult("path"));
            var vm = new FormViewModel(Mock.Of<IDiskIOWrapper>(),
                                       Mock.Of<IStatsEnricher>(),
                                       Mock.Of<IPathValidator>(),
                                       Mock.Of<IRepositorySnapshotBuilderWrapper>(),
                                       fileDialogServiceMock.Object,
                                       Mock.Of<ICoverageReportConverter>())
            {
                RepositoryPath = "bla",
                CoverageCommand = "bla",
                SourceExtensions = "bla",
                SourceRoot = "src",
                CoverageOutputLocation = "bla",
            };

            scheduler.Start(() => vm.OpenFolderDialogCommand
                                    .Execute());

            Helpers.After(TimeSpan.FromMilliseconds(WaitMs),
                          () =>
                          {
                              vm.RepositoryPath
                                .Should()
                                .Be("path");
                          });
        }

        private static bool MatchingRepositoryBuilderArgs(RepositorySnapshotBuilderArguments args) =>
            args.CoveragePath == CoverageOutputLocation &&
            args.RootPath == RepoPath &&
            args.SourceExtensions.First() == ".cs" &&
            args.SourceRoot == SourceRoot;
    }
}
