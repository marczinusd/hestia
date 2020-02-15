using System.Linq;
using static LanguageExt.Prelude;

namespace Hestia.Model.DummyData
{
    public static class DataRepository
    {
        public static readonly Repository DummyRepository = new Repository
        {
            Name = "bla",
            RootDirectory = new Directory
            {
                Directories = Enumerable.Empty<Directory>(),
                Files = new[]
                {
                    new File
                    {
                        Content = new[] { new SourceLine { Text = "bleblabla" }, },
                        Extension = ".js",
                        Filename = "ble",
                        Path = "./ble/",
                    },
                },
                Name = "blabla",
                Path = "/",
            },
            PathToCoverageResultFile = Some("/"),
        };
    }
}
