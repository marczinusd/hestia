using System;
using Autofac;
using Hestia.DAL.Mongo;
using Hestia.DAL.Mongo.Model;
using Hestia.DAL.Mongo.Wrappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;

namespace Hestia.WebService
{
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
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Hestia API", Version = "v1" });
            });

            services.AddSingleton<IMongoClientFactory, MongoClientFactory>();
            services.AddSingleton<ISnapshotPersistence, SnapshotMongoClient>();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<SnapshotMongoClient>()
                   .As<ISnapshotRetrieval>()
                   .WithParameter("databaseName", MongoClientFactory.DatabaseName);
            builder.RegisterInstance<Func<IMongoCollection<RepositorySnapshotEntity>,
                IMongoCollectionWrapper<RepositorySnapshotEntity>>>(entity =>
                                                                        new MongoCollectionWrapper<
                                                                            RepositorySnapshotEntity>(entity));
            builder.RegisterType<MongoClientFactory>()
                   .As<IMongoClientFactory>();
        }

        // ReSharper disable once UnusedMember.Global
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
