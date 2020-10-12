using System.Diagnostics.CodeAnalysis;
using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Hestia.UIRunner.ViewModels;

namespace Hestia.UIRunner.Views
{
    [ExcludeFromCodeCoverage]
    public class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
    }
}
