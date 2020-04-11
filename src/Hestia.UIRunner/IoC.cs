using Autofac;
using Hestia.Model.Builders;
using Hestia.Model.Stats;
using Hestia.Model.Wrappers;
using Hestia.UIRunner.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Hestia.UIRunner
{
    public static class IoC
    {
        public static ContainerBuilder RegisterMainWindowViewModelDependencies(this ContainerBuilder builder)
        {
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
            builder.RegisterType<RepositorySnapshotBuilderWrapper>()
                   .As<IRepositorySnapshotBuilderWrapper>();

            return builder;
        }
    }
}
