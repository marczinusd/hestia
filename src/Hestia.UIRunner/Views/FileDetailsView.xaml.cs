using System.Diagnostics.CodeAnalysis;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Hestia.UIRunner.ViewModels;

namespace Hestia.UIRunner.Views
{
    [ExcludeFromCodeCoverage]
    public class FileDetailsView : ReactiveUserControl<FileDetailsViewModel>
    {
        public FileDetailsView() => InitializeComponent();

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
    }
}
