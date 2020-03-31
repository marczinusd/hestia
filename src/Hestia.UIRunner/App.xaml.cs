using System.Diagnostics.CodeAnalysis;
using Autofac;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Hestia.Model.Builders;
using Hestia.Model.Stats;
using Hestia.Model.Wrappers;
using Hestia.UIRunner.ViewModels;
using Hestia.UIRunner.Views;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

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

        private static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<MainWindowViewModel>();
            builder.RegisterType<DiskIOWrapper>()
                   .As<IDiskIOWrapper>();
            builder.RegisterType<GitCommands>()
                   .As<IGitCommands>();
            builder.RegisterType<CommandLineExecutor>()
                   .As<ICommandLineExecutor>();
            builder.RegisterType<CoverageProviderFactory>()
                   .As<ICoverageProviderFactory>();
            builder.RegisterType<PathValidator>()
                   .As<IPathValidator>();
            builder.RegisterType<ReportGeneratorWrapper>()
                   .As<IReportGeneratorWrapper>();
            builder.RegisterType<CoverageReportConverter>()
                   .As<ICoverageReportConverter>();
            builder.RegisterInstance<ILogger<IStatsEnricher>>(new NullLogger<IStatsEnricher>());
            builder.RegisterType<StatsEnricher>()
                   .As<IStatsEnricher>();

            return builder.Build();
        }
    }
}
