namespace Hestia.Model
{
    public class RepositoryIdentifier
    {
        public RepositoryIdentifier(long id, string name)
        {
            Id = id;
            Name = name;
        }

        public long Id { get; }

        public string Name { get; }
    }
}
