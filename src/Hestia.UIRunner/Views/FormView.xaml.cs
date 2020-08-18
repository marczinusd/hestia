using System.Diagnostics.CodeAnalysis;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Hestia.UIRunner.ViewModels;

namespace Hestia.UIRunner.Views
{
    [ExcludeFromCodeCoverage]
    public class FormView : ReactiveUserControl<FormViewModel>
    {
        public FormView()
        {
            SetupValidation();
            InitializeComponent();
        }

        // ReSharper disable once MemberCanBePrivate.Global
        public TextBlock RepositoryPathValidation => this.FindControl<TextBlock>("RepositoryTextValidation");

        private void SetupValidation() => DataContext = this;

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
    }
}
