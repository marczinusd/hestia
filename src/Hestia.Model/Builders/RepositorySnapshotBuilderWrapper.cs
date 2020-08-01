using System.Diagnostics.CodeAnalysis;
using Hestia.Model.Interfaces;

namespace Hestia.Model.Builders
{
    [ExcludeFromCodeCoverage]
    public class RepositorySnapshotBuilderWrapper : IRepositorySnapshotBuilderWrapper
    {
        public IRepositorySnapshot Build(RepositorySnapshotBuilderArguments args) => args.Build();
    }
}
