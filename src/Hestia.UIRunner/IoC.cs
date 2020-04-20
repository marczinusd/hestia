using Autofac;
using Hestia.Model.Builders;
using Hestia.Model.Stats;
using Hestia.Model.Wrappers;
using Hestia.UIRunner.ViewModels;
using Serilog;

namespace Hestia.UIRunner
{
    public static class IoC
    {
        public static ContainerBuilder RegisterMainWindowViewModelDependencies(this ContainerBuilder builder)
        {
            Log.Logger = new LoggerConfiguration().WriteTo
                                                  .File(@"C:\dev\hestia.log")
                                                  .CreateLogger();

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
            builder.RegisterInstance(Log.Logger);
            builder.RegisterType<StatsEnricher>()
                   .As<IStatsEnricher>();
            builder.RegisterType<RepositorySnapshotBuilderWrapper>()
                   .As<IRepositorySnapshotBuilderWrapper>();
            builder.RegisterType<ReportGeneratorWrapper>()
                   .As<IReportGeneratorWrapper>();
            builder.RegisterType<CoverageReportConverter>()
                   .As<ICoverageReportConverter>();

            return builder;
        }
    }
}
