namespace Hestia.Model.Stats
{
    public class FileGitStats
    {
        // ReSharper disable once UnusedMember.Global
        public FileGitStats()
        {
        }

        public FileGitStats(long lifetimeChanges)
        {
            LifetimeChanges = lifetimeChanges;
        }

        public long Id { get; set; }

        public long LifetimeChanges { get; }
    }
}
