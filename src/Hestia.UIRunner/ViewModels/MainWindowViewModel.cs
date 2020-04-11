using Hestia.Model.Builders;
using Hestia.Model.Stats;
using Hestia.Model.Wrappers;

namespace Hestia.UIRunner.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel(IDiskIOWrapper diskIOWrapper,
                                   IStatsEnricher statsEnricher,
                                   IPathValidator pathValidator,
                                   IRepositorySnapshotBuilderWrapper builderWrapper)
        {
            FormViewModel = new FormViewModel(diskIOWrapper,
                                              statsEnricher,
                                              pathValidator,
                                              builderWrapper);
            RepositoryViewModel = new RepositoryViewModel(FormViewModel.RepositoryCreationObservable);
            FileDetailsViewModel = new FileDetailsViewModel(RepositoryViewModel.SelectedItemObservable);
        }

        public string Greeting => "Hello World!";

        public FormViewModel FormViewModel { get; }

        public RepositoryViewModel RepositoryViewModel { get; }

        public FileDetailsViewModel FileDetailsViewModel { get; }
    }
}
