using Hestia.Model.Wrappers;

namespace Hestia.UIRunner.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string Greeting => "Hello World!";

        public FormViewModel FormViewModel { get; } = new FormViewModel(new DiskIOWrapper());

        public RepositoryViewModel RepositoryViewModel { get; } = new RepositoryViewModel();

        public FileDetailsViewModel FileDetailsViewModel { get; } = new FileDetailsViewModel();
    }
}
