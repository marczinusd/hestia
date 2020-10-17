using System;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Hestia.DAL.EFCore;
using Microsoft.EntityFrameworkCore;

namespace Hestia.UIRunner
{
    [ExcludeFromCodeCoverage]
    internal static class DbSetup
    {
        private const string DBPathConfigKey = "dbPath";
        private const string DBNameConfigKey = "dbName";
        private const string DefaultDBName = "hestia.db";

        private static readonly string DefaultDBFolder =
            Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "dev");

        private static readonly Lazy<HestiaContext> ContextLazy = new Lazy<HestiaContext>(() =>
        {
            var dbPath = BuildDBPath();
            var contextBuilder = new DbContextOptionsBuilder();
            contextBuilder.UseSqlite($@"Data Source={dbPath}");
            var dbContext = new HestiaContext(contextBuilder.Options);
            EnsureDBCreated(dbContext, Path.GetDirectoryName(dbPath));

            return dbContext;
        });

        public static HestiaContext Context => ContextLazy.Value;

        private static string BuildDBPath()
        {
            var dbPathFromConfig = ConfigurationManager.AppSettings[DBPathConfigKey];
            var dbNameFromConfig = ConfigurationManager.AppSettings[DBNameConfigKey];
            var dbFolder = string.IsNullOrWhiteSpace(dbPathFromConfig) ? DefaultDBFolder : dbPathFromConfig;
            var dbName = string.IsNullOrWhiteSpace(dbNameFromConfig) ? DefaultDBName : dbNameFromConfig;

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
