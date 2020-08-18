using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Linq;
using Hestia.Model.Builders;
using Hestia.Model.Interfaces;
using Hestia.Model.Stats;
using Hestia.Model.Wrappers;
using Hestia.UIRunner.Services;
using LanguageExt;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;
using ReactiveUI.Validation.Helpers;
using Unit = System.Reactive.Unit;

namespace Hestia.UIRunner.ViewModels
{
    public class FormViewModel : ReactiveValidationObject<FormViewModel>
    {
        private readonly IRepositorySnapshotBuilderWrapper _builder;
        private readonly IDiskIOWrapper _ioWrapper;
        private readonly ObservableAsPropertyHelper<bool> _isExecuting;
        private readonly IPathValidator _pathValidator;
        private readonly ICoverageReportConverter _reportConverter;
        private readonly IStatsEnricher _statsEnricher;

        public FormViewModel(IDiskIOWrapper ioWrapper,
                             IStatsEnricher statsEnricher,
                             IPathValidator pathValidator,
                             IRepositorySnapshotBuilderWrapper builder,
                             IOpenFileDialogService fileDialogService,
                             ICoverageReportConverter reportConverter)
        {
            _ioWrapper = ioWrapper;
            _statsEnricher = statsEnricher;
            _pathValidator = pathValidator;
            _builder = builder;
            _reportConverter = reportConverter;
            this.ValidationRule(vm => vm.RepositoryPath,
                                ioWrapper.DirectoryExists,
                                "Directory does not exist.");

            this.ValidationRule(vm => vm.RepositoryPath,
                                path => ioWrapper.DirectoryExists(Path.Join(path, ".git")),
                                "Directory is not a git repository");

            EmptyFieldValidation(vm => vm.RepositoryPath, nameof(RepositoryPath));
            EmptyFieldValidation(vm => vm.SourceExtensions, nameof(SourceExtensions));
            EmptyFieldValidation(vm => vm.CoverageOutputLocation, nameof(CoverageOutputLocation));

            ProcessRepositoryCommand =
                ReactiveCommand.CreateFromObservable(BuildSnapshotFromViewModelState,
                                                     this.WhenAnyValue(vm => vm.HasErrors,
                                                                       hasErrors =>
                                                                           !hasErrors));
            ProcessRepositoryCommand.IsExecuting.ToProperty(this, x => x.IsExecuting, out _isExecuting);

            _isExecuting = ProcessRepositoryCommand.IsExecuting.ToProperty(this, nameof(IsExecuting));

            OpenFolderDialogCommand =
                ReactiveCommand.CreateFromTask<Unit, string>(_ => fileDialogService.OpenFolderDialog());

            OpenFileDialogCommand =
                ReactiveCommand.CreateFromTask<Unit, string>(_ => fileDialogService.OpenFileDialog());

            OpenFileDialogCommand.AsObservable()
                                 .Subscribe(result => CoverageOutputLocation = result);

            OpenFolderDialogCommand.AsObservable()
                                   .Subscribe(result => RepositoryPath = result);
        }

        [Reactive] public string RepositoryPath { get; set; }

        [Reactive] public string SourceExtensions { get; set; }

        [Reactive] public string SourceRoot { get; set; }

        [Reactive] public string CoverageCommand { get; set; }

        [Reactive] public string CoverageOutputLocation { get; set; }

        public bool IsExecuting => _isExecuting.Value;

        public ReactiveCommand<Unit, IRepositorySnapshot> ProcessRepositoryCommand { get; set; }

        public ReactiveCommand<Unit, string> OpenFolderDialogCommand { get; set; }

        public ReactiveCommand<Unit, string> OpenFileDialogCommand { get; set; }

        public IObservable<IRepositorySnapshot> RepositoryCreationObservable => ProcessRepositoryCommand.AsObservable();

        private RepositorySnapshotBuilderArguments FieldsAsBuilderArguments() =>
            new RepositorySnapshotBuilderArguments(string.Empty,
                                                   RepositoryPath,
                                                   SourceRoot,
                                                   SourceExtensions.Split(";")
                                                                   .Select(x => x.Trim())
                                                                   .ToArray(),
                                                   CoverageOutputLocation,
                                                   Option<string>.None,
                                                   Option<DateTime>.None,
                                                   _ioWrapper,
                                                   _pathValidator);

        private void EmptyFieldValidation(Expression<Func<FormViewModel, string>> func,
                                          string fieldName) =>
            this.ValidationRule(func, s => !string.IsNullOrWhiteSpace(s), $"{fieldName} should not be empty");

        private IObservable<IRepositorySnapshot> BuildSnapshotFromViewModelState() =>
            Observable.Start(() => FieldsAsBuilderArguments()
                                   .Apply(_builder.Build)
                                   .Apply(ConvertCoverageReport)
                                   .Apply(_statsEnricher.EnrichWithCoverage)
                                   .Apply(x => _statsEnricher.EnrichWithGitStats(x, GitStatGranularity.File)));

        private IRepositorySnapshot ConvertCoverageReport(IRepositorySnapshot snapshot)
        {
            var newPath = _reportConverter
                          .Convert(CoverageOutputLocation,
                                   Path.GetDirectoryName(CoverageOutputLocation)!)
                          .Some(x => x)
                          .None(() => null);

            return snapshot.With(pathToCoverageResultFile: newPath);
        }
    }
}
