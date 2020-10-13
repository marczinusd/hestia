using Autofac;
using Hestia.DAL.EFCore;
using Hestia.DAL.Interfaces;
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
                                                  .File(@"hestia.log")
                                                  .CreateLogger();

            builder.RegisterType<MainWindowViewModel>();
            builder.RegisterType<DiskIOWrapper>()
                   .As<IDiskIOWrapper>();
            builder.RegisterType<GitCommands>()
                   .As<IGitCommands>();
            builder.RegisterType<CommandLineExecutor>()
                   .As<ICommandLineExecutor>()
                   .WithParameter("echoMode", ExecutorEchoMode.NoEcho);
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

        public static ContainerBuilder WithDbConnection(this ContainerBuilder builder, HestiaContext dbContext)
        {
            builder.RegisterType<SnapshotEFClient>()
                   .As<ISnapshotPersistence>();
            builder.RegisterType<XmlFileSerializationWrapper>()
                   .As<IXmlFileSerializationWrapper>();
            builder.RegisterInstance(dbContext);

            return builder;
        }
    }
}
