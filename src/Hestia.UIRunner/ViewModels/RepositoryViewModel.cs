using System;
using Hestia.Model;
using ReactiveUI;

namespace Hestia.UIRunner.ViewModels
{
    public class RepositoryViewModel : ViewModelBase
    {
        private readonly ObservableAsPropertyHelper<RepositorySnapshot> selectedRepository;

        public RepositoryViewModel(IObservable<RepositorySnapshot> selectedRepositoryObservable)
        {
            selectedRepository = selectedRepositoryObservable.ToProperty(this, nameof(Repository));
        }

        public RepositorySnapshot Repository => selectedRepository.Value;

        public File SelectedItem
        {
            get;
            set;
        }
    }
}
