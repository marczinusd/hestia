using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Hestia.DAL.EFCore;
using Hestia.DAL.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;

namespace Hestia.WebService
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient(provider =>
            {
                var optionsBuilder = new DbContextOptionsBuilder();
                var dbPath = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "dev");
                var dbConn = Configuration["Connection:SqliteConn"];
                optionsBuilder.UseSqlite($"Data Source={Path.Join(dbPath, dbConn)}");
                var context = new HestiaContext(optionsBuilder.Options);
                if (!Directory.Exists(dbPath))
                {
                    Directory.CreateDirectory(dbPath);
                }

                context.Database.EnsureCreated();

                return new HestiaContext(optionsBuilder.Options);
            });
            services.AddTransient<ISnapshotRetrieval, SnapshotEFClient>();
            services.AddTransient<IFileRetrieval, SnapshotEFClient>();
            services.AddSingleton<ILogger>(_ => Log.Logger);
            services.AddCors();
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Hestia API", Version = "v1" });
            });
        }

        // ReSharper disable once UnusedMember.Global
        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hestia API v1");
                c.RoutePrefix = string.Empty;
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors(options =>
                {
                    options.AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowAnyOrigin()
                           .AllowCredentials();
                });
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
