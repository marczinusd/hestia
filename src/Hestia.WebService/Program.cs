using System.Diagnostics.CodeAnalysis;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Hestia.WebService
{
    // ReSharper disable once ClassNeverInstantiated.Global
    [ExcludeFromCodeCoverage]
    public static class Program
    {
        public static void Main(string[] args) =>
            CreateHostBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .Build()
                .Run();

        // ReSharper disable once MemberCanBePrivate.Global
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
    }
}
