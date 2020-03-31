using System;
using System.IO;
using System.Linq.Expressions;
using System.Reactive.Linq;
using Hestia.Model;
using Hestia.Model.Stats;
using Hestia.Model.Wrappers;
using LanguageExt;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;
using ReactiveUI.Validation.Helpers;

namespace Hestia.UIRunner.ViewModels
{
    public class FormViewModel : ReactiveValidationObject<FormViewModel>
    {
        private readonly IStatsEnricher _statsEnricher;

        public FormViewModel(IDiskIOWrapper ioWrapper, IStatsEnricher statsEnricher)
        {
            _statsEnricher = statsEnricher;
            this.ValidationRule(vm => vm.RepositoryPath,
                                ioWrapper.DirectoryExists,
                                "Directory does not exist.");

            this.ValidationRule(vm => vm.RepositoryPath,
                                path => ioWrapper.DirectoryExists(Path.Join(path, ".git")),
                                "Directory is not a git repository");

            EmptyFieldValidation(vm => vm.CoverageCommand, nameof(CoverageCommand));
            EmptyFieldValidation(vm => vm.RepositoryPath, nameof(RepositoryPath));
            EmptyFieldValidation(vm => vm.SourceExtensions, nameof(SourceExtensions));
            EmptyFieldValidation(vm => vm.CoverageOutputLocation, nameof(CoverageOutputLocation));

            ProcessRepositoryCommand =
                ReactiveCommand.Create<Unit, RepositorySnapshot>(_ => new RepositorySnapshot(1,
                                                                                             null,
                                                                                             Option<string>.None,
                                                                                             Option<string>.None,
                                                                                             Option<DateTime>.None),
                                                                 this.WhenAnyValue(vm => vm.HasErrors,
                                                                                   hasErrors => !hasErrors));
        }

        [Reactive] public string RepositoryPath { get; set; }

        [Reactive] public string SourceExtensions { get; set; }

        [Reactive] public string CoverageCommand { get; set; }

        [Reactive] public string CoverageOutputLocation { get; set; }

        public ReactiveCommand<Unit, RepositorySnapshot> ProcessRepositoryCommand { get; set; }

        public IObservable<RepositorySnapshot> RepositoryCreationObservable => ProcessRepositoryCommand.AsObservable();

        private void EmptyFieldValidation(Expression<Func<FormViewModel, string>> func,
                                          string fieldName) =>
            this.ValidationRule(func, s => !string.IsNullOrWhiteSpace(s), $"{fieldName} should not be empty");
    }
}
