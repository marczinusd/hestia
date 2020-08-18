using System.Diagnostics.CodeAnalysis;
using Autofac;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Hestia.UIRunner.Services;
using Hestia.UIRunner.ViewModels;
using Hestia.UIRunner.Views;

namespace Hestia.UIRunner
{
    [ExcludeFromCodeCoverage]
    public class App : Application
    {
        public override void Initialize() => AvaloniaXamlLoader.Load(this);

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var builder = BuildContainer();
                desktop.MainWindow = new MainWindow();
                builder.RegisterInstance<IOpenFileDialogService>(new OpenFileDialogService(() => desktop.MainWindow));
                desktop.MainWindow.DataContext = builder.Build()
                                                        .Resolve<MainWindowViewModel>();
            }

            base.OnFrameworkInitializationCompleted();
        }

        private static ContainerBuilder BuildContainer() =>
            new ContainerBuilder()
                .RegisterMainWindowViewModelDependencies();
    }
}
