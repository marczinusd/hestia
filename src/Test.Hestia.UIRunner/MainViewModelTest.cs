using FluentAssertions;
using Hestia.Model.Builders;
using Hestia.Model.Stats;
using Hestia.Model.Wrappers;
using Hestia.UIRunner.ViewModels;
using Moq;
using Xunit;

namespace Test.Hestia.UIRunner
{
    public class MainViewModelTest
    {
        [Fact]
        public void MainViewModelShowHaveExpectedGreeting()
        {
            new MainWindowViewModel(new DiskIOWrapper(),
                                    Mock.Of<IStatsEnricher>(),
                                    Mock.Of<IPathValidator>(),
                                    Mock.Of<IRepositorySnapshotBuilderWrapper>())
                .Greeting
                .Should()
                .Be("Hello World!");
        }
    }
}
