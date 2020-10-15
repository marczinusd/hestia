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
        public static readonly Lazy<HestiaContext> Context = new Lazy<HestiaContext>(() =>
        {
            var dbPathFromConfig = ConfigurationManager.AppSettings["dbPath"];
            var dbNameFromConfig = ConfigurationManager.AppSettings["dbName"];
            var dbFolder = string.IsNullOrWhiteSpace(dbPathFromConfig)
                               ? Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "dev")
                               : dbPathFromConfig;
            var dbName = string.IsNullOrWhiteSpace(dbNameFromConfig) ? "hestia.db" : dbNameFromConfig;

            var contextBuilder = new DbContextOptionsBuilder();
            contextBuilder.UseSqlite($@"Data Source={Path.Join(dbFolder, dbName)}");
            var dbContext = new HestiaContext(contextBuilder.Options);
            EnsureDBCreated(dbContext, dbFolder);

            return dbContext;
        });

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
