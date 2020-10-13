using System;
using System.IO;
using Hestia.DAL.EFCore;
using Microsoft.EntityFrameworkCore;

namespace Hestia.UIRunner
{
    internal static class DbSetup
    {
        public static readonly Lazy<HestiaContext> Context = new Lazy<HestiaContext>(() =>
        {
            var contextBuilder = new DbContextOptionsBuilder();
            var dbFolder = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "dev");
            var dbPath = Path.Join(dbFolder, "hestia.db");
            if (!Directory.Exists(dbFolder))
            {
                Directory.CreateDirectory(dbFolder);
            }

            contextBuilder.UseSqlite($@"Data Source={dbPath}"); // TODO: move this to app.config
            var dbContext = new HestiaContext(contextBuilder.Options);
            dbContext.Database.EnsureCreated();

            return dbContext;
        });
    }
}
