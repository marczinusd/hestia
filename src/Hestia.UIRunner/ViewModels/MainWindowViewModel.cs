using Hestia.Model.Wrappers;

namespace Hestia.UIRunner.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string Greeting => "Hello World!";

        public FormViewModel FormViewModel { get; } = new FormViewModel(new DiskIOWrapper());
    }
}
