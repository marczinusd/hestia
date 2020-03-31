using Hestia.Model.Stats;
using Hestia.Model.Wrappers;

namespace Hestia.UIRunner.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel(IDiskIOWrapper diskIOWrapper, IStatsEnricher statsEnricher)
        {
            FormViewModel = new FormViewModel(diskIOWrapper, statsEnricher);
            RepositoryViewModel = new RepositoryViewModel(FormViewModel.RepositoryCreationObservable);
            FileDetailsViewModel = new FileDetailsViewModel();
        }

        public string Greeting => "Hello World!";

        public FormViewModel FormViewModel { get; }

        public RepositoryViewModel RepositoryViewModel { get; }

        public FileDetailsViewModel FileDetailsViewModel { get; }
    }
}
