using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Linq;
using Hestia.Model;
using Hestia.Model.Builders;
using Hestia.Model.Stats;
using Hestia.Model.Wrappers;
using Hestia.UIRunner.Services;
using LanguageExt;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;
using ReactiveUI.Validation.Helpers;
using Path = System.IO.Path;

namespace Hestia.UIRunner.ViewModels
{
    public class FormViewModel : ReactiveValidationObject<FormViewModel>
    {
        private readonly IStatsEnricher _statsEnricher;
        private readonly IPathValidator _pathValidator;
        private readonly IRepositorySnapshotBuilderWrapper _builder;
        private readonly ICoverageReportConverter _reportConverter;
        private readonly IDiskIOWrapper _ioWrapper;
        private readonly ObservableAsPropertyHelper<bool> _isExecuting;

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

            // Removed for now
            // EmptyFieldValidation(vm => vm.CoverageCommand, nameof(CoverageCommand));
            EmptyFieldValidation(vm => vm.RepositoryPath, nameof(RepositoryPath));
            EmptyFieldValidation(vm => vm.SourceExtensions, nameof(SourceExtensions));
            EmptyFieldValidation(vm => vm.CoverageOutputLocation, nameof(CoverageOutputLocation));

            ProcessRepositoryCommand =
                ReactiveCommand.Create<Unit, RepositorySnapshot>(_ => BuildSnapshotFromViewModelState(),
                                                                 this.WhenAnyValue(vm => vm.HasErrors,
                                                                                   hasErrors => !hasErrors));

            _isExecuting = ProcessRepositoryCommand.IsExecuting.ToProperty(this, nameof(IsExecuting));

            OpenFolderDialogCommand =
                ReactiveCommand.CreateFromTask<Unit, string>(_ => fileDialogService.OpenFolderDialog());

            OpenFileDialogCommand =
                ReactiveCommand.CreateFromTask<Unit, string[]>(_ => fileDialogService.OpenFileDialog());

            OpenFileDialogCommand.AsObservable()
                                 .Subscribe(result => CoverageOutputLocation =
                                                          result.Any() ? result.Single() : CoverageOutputLocation);

            OpenFolderDialogCommand.AsObservable()
                                   .Subscribe(result => RepositoryPath =
                                                            string.IsNullOrEmpty(result) ? RepositoryPath : result);
        }

        [Reactive] public string RepositoryPath { get; set; }

        [Reactive] public string SourceExtensions { get; set; }

        [Reactive] public string SourceRoot { get; set; }

        [Reactive] public string CoverageCommand { get; set; }

        [Reactive] public string CoverageOutputLocation { get; set; }

        public bool IsExecuting => _isExecuting.Value;

        public ReactiveCommand<Unit, RepositorySnapshot> ProcessRepositoryCommand { get; set; }

        public ReactiveCommand<Unit, string> OpenFolderDialogCommand { get; set; }

        public ReactiveCommand<Unit, string[]> OpenFileDialogCommand { get; set; }

        public IObservable<RepositorySnapshot> RepositoryCreationObservable => ProcessRepositoryCommand.AsObservable();

        private RepositorySnapshotBuilderArguments FieldsAsBuilderArguments =>
            new RepositorySnapshotBuilderArguments(-1,
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

        private RepositorySnapshot BuildSnapshotFromViewModelState() =>
            FieldsAsBuilderArguments
                .Apply(_builder.Build)
                .Apply(ConvertCoverageReport)
                .Apply(_statsEnricher.EnrichWithCoverage)
                .Apply(x => _statsEnricher.EnrichWithGitStats(x));

        private RepositorySnapshot ConvertCoverageReport(RepositorySnapshot snapshot)
        {
            var newPath = _reportConverter
                          .Convert(CoverageOutputLocation, Path.GetDirectoryName(CoverageOutputLocation))
                          .Match(val => val, () => null);

            return snapshot.With(pathToCoverageResultFile: newPath);
        }
    }
}
