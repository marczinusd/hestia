using System;
using FluentAssertions;
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
            var controller = new RepositoriesController(Mock.Of<ILogger<RepositoriesController>>());

            Action act1 = () => controller.GetAllRepositories();
            Action act2 = () => controller.GetRepositoryById(1);
            Action act3 = () => controller.GetFileById(1, 2);

            act1.Should()
                .Throw<NotImplementedException>();
            act2.Should()
                .Throw<NotImplementedException>();
            act3.Should()
                .Throw<NotImplementedException>();
        }
    }
}
