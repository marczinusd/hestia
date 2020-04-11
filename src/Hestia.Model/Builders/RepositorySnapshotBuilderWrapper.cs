namespace Hestia.Model.Builders
{
    public class RepositorySnapshotBuilderWrapper : IRepositorySnapshotBuilderWrapper
    {
        public RepositorySnapshot Build(RepositorySnapshotBuilderArguments args) => args.Build();
    }
}
