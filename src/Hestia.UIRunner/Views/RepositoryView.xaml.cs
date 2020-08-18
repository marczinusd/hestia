using System.Diagnostics.CodeAnalysis;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Hestia.UIRunner.ViewModels;

namespace Hestia.UIRunner.Views
{
    [ExcludeFromCodeCoverage]
    public class RepositoryView : ReactiveUserControl<RepositoryViewModel>
    {
        public RepositoryView() => InitializeComponent();

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
    }
}
