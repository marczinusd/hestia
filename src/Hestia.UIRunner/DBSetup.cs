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
            var dbPath = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "dev", "hestia.db");
            contextBuilder.UseSqlite($@"Data Source={dbPath}"); // TODO: move this to app.config

            var dbContext = new HestiaContext(contextBuilder.Options);
            dbContext.Database.EnsureCreated();

            return dbContext;
        });
    }
}
