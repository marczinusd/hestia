namespace Hestia.Model.Stats
{
    public class FileGitStats
    {
        public FileGitStats(long lifetimeChanges)
        {
            LifetimeChanges = lifetimeChanges;
        }

        public long LifetimeChanges { get; }
    }
}
