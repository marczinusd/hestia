using System.Diagnostics.CodeAnalysis;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Hestia.UIRunner.Views
{
    [ExcludeFromCodeCoverage]
    public class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
