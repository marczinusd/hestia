using System.Diagnostics.CodeAnalysis;
using Autofac;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Hestia.UIRunner.ViewModels;
using Hestia.UIRunner.Views;

namespace Hestia.UIRunner
{
    [ExcludeFromCodeCoverage]
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var container = BuildContainer();
                desktop.MainWindow = new MainWindow { DataContext = container.Resolve<MainWindowViewModel>(), };
            }

            base.OnFrameworkInitializationCompleted();
        }

        private static IContainer BuildContainer() =>
            new ContainerBuilder()
                .RegisterMainWindowViewModelDependencies()
                .Build();
    }
}
