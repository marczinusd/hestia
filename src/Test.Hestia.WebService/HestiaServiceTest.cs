using System;
using Autofac.Extensions.DependencyInjection;
using FluentAssertions;
using Hestia.DAL.Interfaces;
using Hestia.WebService;
using Hestia.WebService.Controllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Test.Hestia.WebService
{
    public class HestiaServiceTest
    {
        // TODO: For now just test whether this setup is correct -- implement later
        [Fact]
        public void SmokeTest()
        {
            var server = new TestServer(new WebHostBuilder().ConfigureServices(services => services.AddAutofac())
                                                            .UseStartup<Startup>());
            server.CreateClient();
            var controller =
                new SnapshotsController(Mock.Of<ILogger<SnapshotsController>>(), Mock.Of<ISnapshotRetrieval>());

            Action act1 = () => controller.GetAllRepositories();
            Action act2 = () => controller.GetRepositoryById(string.Empty);

            act1.Should()
                .NotThrow<NotImplementedException>();
            act2.Should()
                .NotThrow<NotImplementedException>();
        }
    }
}
