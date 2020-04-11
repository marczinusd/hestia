using System;
using Hestia.Model;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Hestia.UIRunner.ViewModels
{
    public class RepositoryViewModel : ViewModelBase
    {
        private readonly ObservableAsPropertyHelper<RepositorySnapshot> selectedRepository;

        public RepositoryViewModel(IObservable<RepositorySnapshot> selectedRepositoryObservable)
        {
            selectedRepository = selectedRepositoryObservable.ToProperty(this, nameof(Repository));
            SelectedItemObservable = this.WhenAnyValue(x => x.SelectedItem);
        }

        public RepositorySnapshot Repository => selectedRepository.Value;

        public IObservable<File> SelectedItemObservable { get; }

        [Reactive]
        public File SelectedItem
        {
            get;
            set;
        }
    }
}
