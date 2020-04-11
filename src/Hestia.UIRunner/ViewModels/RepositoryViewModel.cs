using System;
using System.Collections.Generic;
using Hestia.Model;
using JetBrains.Annotations;
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
        }

        public RepositorySnapshot Repository => selectedRepository.Value;

        public File SelectedItem
        {
            get;
            set;
        }
    }
}
