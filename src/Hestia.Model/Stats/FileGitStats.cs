namespace Hestia.Model.Stats
{
    public class FileGitStats
    {
        public FileGitStats(int lifetimeChanges, int lifetimeAuthors)
        {
            LifetimeChanges = lifetimeChanges;
            LifetimeAuthors = lifetimeAuthors;
        }

        public int LifetimeChanges { get; }

        public int LifetimeAuthors { get; }
    }
}
