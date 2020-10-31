using System;
using System.IO;
using Hestia.DAL.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Hestia.ConsoleRunner
{
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
