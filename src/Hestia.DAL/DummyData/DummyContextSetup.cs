using Hestia.DAL.Entities;

namespace Hestia.DAL.DummyData
{
    public static class DummyContextSetup
    {
        public static void Setup(this HestiaContext context)
        {
            var repository = new RepositoryEntity();
            repository.Name = "DummyEntity";
            repository.RootDirectory = new DirectoryEntity();

            var fileEntity1 = new FileEntity();
            fileEntity1.Extension = ".js";
            fileEntity1.Filename = "hello";
            fileEntity1.Path = "./src/";
            fileEntity1.Content.Add(new LineEntity { Text = "hello!" });

            var fileEntity2 = new FileEntity();
            fileEntity2.Extension = ".txt";
            fileEntity2.Filename = "hello";
            fileEntity2.Path = "./install/";
            fileEntity2.Content.Add(new LineEntity { Text = "hello!" });

            var directoryEntity1 = new DirectoryEntity();
            directoryEntity1.Name = "src";
            directoryEntity1.Path = "./";
            directoryEntity1.Files.Add(fileEntity1);

            var directoryEntity2 = new DirectoryEntity();
            directoryEntity2.Name = "install";
            directoryEntity2.Path = "./";
            directoryEntity2.Files.Add(fileEntity2);
            repository.RootDirectory.Directories.Add(directoryEntity1);
            repository.RootDirectory.Directories.Add(directoryEntity2);

            context.Repositories.Add(repository);
            context.SaveChanges();
        }
    }
}
