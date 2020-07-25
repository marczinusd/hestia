using System.Diagnostics.CodeAnalysis;

namespace Hestia.Model.Builders
{
    [ExcludeFromCodeCoverage]
    public class RepositorySnapshotBuilderWrapper : IRepositorySnapshotBuilderWrapper
    {
        public RepositorySnapshot Build(RepositorySnapshotBuilderArguments args) => args.Build();
    }
}
