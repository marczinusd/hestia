using FluentAssertions;
using Hestia.UIRunner.ViewModels;
using Xunit;

namespace Test.Hestia.UIRunner
{
    public class MainViewModelTest
    {
        [Fact]
        public void MainViewModelShowHaveExpectedGreeting()
        {
            new MainWindowViewModel().Greeting
                                     .Should()
                                     .Be("Hello World!");
        }
    }
}
