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
        private readonly ObservableAsPropertyHelper<bool> _isExecuting;
        private readonly ObservableAsPropertyHelper<IRepositorySnapshot> _selectedRepository;

        public RepositoryViewModel(IObservable<IRepositorySnapshot> selectedRepositoryObservable,
                                   ISnapshotPersistence snapshotPersistence)
        {
            _selectedRepository = selectedRepositoryObservable.ToProperty(this, nameof(Repository));
            SelectedItemObservable = this.WhenAnyValue(x => x.SelectedItem);
            CommitToDatabaseCommand = ReactiveCommand.CreateFromObservable<Unit, Unit>(_ =>
                snapshotPersistence.InsertSnapshot(_selectedRepository.Value));
            CommitToDatabaseCommand.IsExecuting.ToProperty(this, x => x.IsInsertionInProgress, out _isExecuting);
        }

        public bool IsInsertionInProgress => _isExecuting.Value;

        public IRepositorySnapshot Repository => _selectedRepository.Value;

        public ReactiveCommand<Unit, Unit> CommitToDatabaseCommand { get; }

        public IObservable<IFile> SelectedItemObservable { get; }

        [Reactive]
        public File SelectedItem
        {
            get;
            set;
        }
    }
}
