using Hestia.Model.Interfaces;

namespace Hestia.Model.Stats
{
    public class FileGitStats : IFileGitStats
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
