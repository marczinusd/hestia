using Hestia.Model.Wrappers;

namespace Hestia.UIRunner.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string Greeting => "Hello World!";

        public RepositoryFormViewModel FormViewModel { get; } = new RepositoryFormViewModel(new DiskIOWrapper());
    }
}
