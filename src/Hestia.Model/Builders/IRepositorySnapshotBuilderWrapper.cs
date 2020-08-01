using Hestia.Model.Interfaces;

namespace Hestia.Model.Builders
{
    public interface IRepositorySnapshotBuilderWrapper
    {
        IRepositorySnapshot Build(RepositorySnapshotBuilderArguments args);
    }
}
