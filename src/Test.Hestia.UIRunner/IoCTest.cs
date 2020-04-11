using Autofac;
using FluentAssertions;
using Hestia.UIRunner;
using Xunit;

namespace Test.Hestia.UIRunner
{
    public class IoCTest
    {
        [Fact]
        public void MainWindowViewModelCanBeResolved() =>
            new ContainerBuilder()
                .RegisterMainWindowViewModelDependencies()
                .Build()
                .Should()
                .NotBeNull();
    }
}
