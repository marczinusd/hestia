using System;
using System.Reactive;
using System.Reactive.Linq;
using Hestia.DAL.Mongo;
using Hestia.Model;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Hestia.UIRunner.ViewModels
{
    public class RepositoryViewModel : ReactiveObject
    {
        private readonly ObservableAsPropertyHelper<RepositorySnapshot> selectedRepository;

        public RepositoryViewModel(IObservable<RepositorySnapshot> selectedRepositoryObservable,
                                   ISnapshotPersistence snapshotPersistence)
        {
            selectedRepository = selectedRepositoryObservable.ToProperty(this, nameof(Repository));
            SelectedItemObservable = this.WhenAnyValue(x => x.SelectedItem);
            CommitToDatabaseCommand = ReactiveCommand.Create(() =>
            {
                snapshotPersistence.InsertSnapshot(selectedRepository.Value);
            }, selectedRepositoryObservable.Select(x => x != null));
        }

        public RepositorySnapshot Repository => selectedRepository.Value;

        public ReactiveCommand<Unit, Unit> CommitToDatabaseCommand { get; }

        public IObservable<File> SelectedItemObservable { get; }

        [Reactive]
        public File SelectedItem
        {
            get;
            set;
        }
    }
}
