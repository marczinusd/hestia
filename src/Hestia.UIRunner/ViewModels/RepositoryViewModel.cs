using System;
using Hestia.DAL.Interfaces;
using Hestia.Model;
using Hestia.Model.Interfaces;
using LanguageExt;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Hestia.UIRunner.ViewModels
{
    public class RepositoryViewModel : ReactiveObject
    {
        private readonly ObservableAsPropertyHelper<IRepositorySnapshot> _selectedRepository;
        private readonly ObservableAsPropertyHelper<bool> _isExecuting;

        public RepositoryViewModel(IObservable<IRepositorySnapshot> selectedRepositoryObservable,
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

        public IRepositorySnapshot Repository => _selectedRepository.Value;

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
