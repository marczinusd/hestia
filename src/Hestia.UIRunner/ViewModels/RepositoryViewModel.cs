using System;
using System.Reactive;
using Hestia.DAL.Mongo;
using Hestia.Model;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Hestia.UIRunner.ViewModels
{
    public class RepositoryViewModel : ReactiveObject
    {
        private readonly ObservableAsPropertyHelper<RepositorySnapshot> _selectedRepository;
        private readonly ObservableAsPropertyHelper<bool> _isExecuting;

        public RepositoryViewModel(IObservable<RepositorySnapshot> selectedRepositoryObservable,
                                   ISnapshotPersistence snapshotPersistence)
        {
            _selectedRepository = selectedRepositoryObservable.ToProperty(this, nameof(Repository));
            SelectedItemObservable = this.WhenAnyValue(x => x.SelectedItem);
            CommitToDatabaseCommand =
                ReactiveCommand.CreateFromObservable<Unit, Unit>(_ => snapshotPersistence
                                                                     .InsertSnapshot(_selectedRepository
                                                                                         .Value));
            CommitToDatabaseCommand.IsExecuting.ToProperty(this, x => x.IsInsertionInProgress, out _isExecuting);
        }

        public bool IsInsertionInProgress => _isExecuting.Value;

        public RepositorySnapshot Repository => _selectedRepository.Value;

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
