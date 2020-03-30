using System.Diagnostics.CodeAnalysis;
using System.Reactive.Disposables;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Hestia.UIRunner.ViewModels;
using ReactiveUI;
using ReactiveUI.Validation.Extensions;

namespace Hestia.UIRunner.Views
{
    [ExcludeFromCodeCoverage]
    public class FormView : ReactiveUserControl<RepositoryFormViewModel>
    {
        public FormView()
        {
            SetupValidation();
            InitializeComponent();
        }

        // ReSharper disable once MemberCanBePrivate.Global
        public TextBlock RepositoryPathValidation => this.FindControl<TextBlock>("RepositoryTextValidation");

        private void SetupValidation()
        {
            this.WhenActivated(disposables =>
            {
                // Bind the 'ExampleCommand' to 'ExampleButton' defined above.
                this.BindValidation(ViewModel, x => x.RepositoryPath, x => x.RepositoryPathValidation)
                    .DisposeWith(disposables);
            });
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
