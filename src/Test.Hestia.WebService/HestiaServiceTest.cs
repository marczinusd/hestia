using System;
using FluentAssertions;
using Hestia.DAL.Mongo;
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
            var server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            server.CreateClient();
            var controller = new SnapshotsController(Mock.Of<ILogger<SnapshotsController>>(), Mock.Of<ISnapshotRetrieval>());

            Action act1 = () => controller.GetAllRepositories();
            Action act2 = () => controller.GetRepositoryById(string.Empty);

            act1.Should()
                .Throw<NotImplementedException>();
            act2.Should()
                .Throw<NotImplementedException>();
        }
    }
}
