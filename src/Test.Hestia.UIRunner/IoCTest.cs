using Autofac;
using FluentAssertions;
using Hestia.DAL.EFCore;
using Hestia.UIRunner;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Test.Hestia.UIRunner
{
    public class IoCTest
    {
        [Fact]
        public void MainWindowViewModelCanBeResolved() =>
            new ContainerBuilder()
                .RegisterMainWindowViewModelDependencies()
                .WithDbConnection(new HestiaContext(new DbContextOptionsBuilder().Options))
                .Build()
                .Should()
                .NotBeNull();
    }
}
