using System;
using Hestia.WebService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Xunit;
using Xunit.Abstractions;

namespace Test.Hestia.WebService
{
    public class HestiaServiceTest
    {
        private readonly ITestOutputHelper _output;

        public HestiaServiceTest(ITestOutputHelper output)
        {
            _output = output;
        }

        // For now just test whether this setup is correct -- implement later
        [Fact]
        public void SmokeTest()
        {
            var server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            var client = server.CreateClient();

            _output.WriteLine(client.ToString());
        }
    }
}
