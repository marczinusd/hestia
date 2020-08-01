using Hestia.DAL.Interfaces;
using Hestia.Model.Builders;
using Hestia.Model.Stats;
using Hestia.Model.Wrappers;
using Hestia.UIRunner.Services;
using ReactiveUI;

namespace Hestia.UIRunner.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        public MainWindowViewModel(IDiskIOWrapper diskIOWrapper,
                                   IStatsEnricher statsEnricher,
                                   IPathValidator pathValidator,
                                   IRepositorySnapshotBuilderWrapper builderWrapper,
                                   IOpenFileDialogService fileDialogService,
                                   ICoverageReportConverter converter,
                                   ISnapshotPersistence snapshotPersistence)
        {
            FormViewModel = new FormViewModel(diskIOWrapper,
                                              statsEnricher,
                                              pathValidator,
                                              builderWrapper,
                                              fileDialogService,
                                              converter);
            RepositoryViewModel = new RepositoryViewModel(FormViewModel.RepositoryCreationObservable, snapshotPersistence);
            FileDetailsViewModel = new FileDetailsViewModel(RepositoryViewModel.SelectedItemObservable);
        }

        public string Greeting => "Hello World!";

        public FormViewModel FormViewModel { get; }

        public RepositoryViewModel RepositoryViewModel { get; }

        public FileDetailsViewModel FileDetailsViewModel { get; }
    }
}
