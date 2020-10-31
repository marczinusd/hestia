using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Autofac;
using Hestia.DAL.EFCore;
using Hestia.DAL.Interfaces;
using Hestia.Model.Wrappers;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Hestia.ConsoleRunner
{
    [ExcludeFromCodeCoverage]
    internal static class DbSetup
    {
        private const string DefaultDBName = "hestia.db";

        private static readonly string DefaultDBFolder =
            Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "dev");

        public static HestiaContext CreateContext(string dbName, string dbPath)
        {
            var finalPath = BuildDBPath(dbName, dbPath);
            Log.Logger.Information($"Using sqlite db at {finalPath}");
            var contextBuilder = new DbContextOptionsBuilder();
            contextBuilder.UseSqlite($@"Data Source={finalPath}");
            var dbContext = new HestiaContext(contextBuilder.Options);
            EnsureDBCreated(dbContext, Path.GetDirectoryName(finalPath)!);

            return dbContext;
        }

        public static ContainerBuilder WithDbContext(this ContainerBuilder builder, string dbName, string dbPath)
        {
            builder.RegisterType<SnapshotEFClient>()
                   .As<ISnapshotPersistence>();
            builder.RegisterType<XmlFileSerializationWrapper>()
                   .As<IXmlFileSerializationWrapper>();
            builder.RegisterInstance(CreateContext(dbName, dbPath));

            return builder;
        }

        private static string BuildDBPath(string name, string path)
        {
            var dbFolder = string.IsNullOrWhiteSpace(path) ? DefaultDBFolder : path;
            var dbName = string.IsNullOrWhiteSpace(name) ? DefaultDBName : name;

            return Path.Join(dbFolder, dbName);
        }

        private static void EnsureDBCreated(HestiaContext context, string dbFolder)
        {
            if (!Directory.Exists(dbFolder))
            {
                Directory.CreateDirectory(dbFolder);
            }

            context.Database.EnsureCreated();
        }
    }
}
