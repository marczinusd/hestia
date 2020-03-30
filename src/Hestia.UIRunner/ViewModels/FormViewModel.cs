using System;
using System.IO;
using System.Linq.Expressions;
using Hestia.Model.Wrappers;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;
using ReactiveUI.Validation.Helpers;

namespace Hestia.UIRunner.ViewModels
{
    public class FormViewModel : ReactiveValidationObject<FormViewModel>
    {
        public FormViewModel(IDiskIOWrapper ioWrapper)
        {
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
        }

        [Reactive] public string RepositoryPath { get; set; }

        [Reactive] public string SourceExtensions { get; set; }

        [Reactive] public string CoverageCommand { get; set; }

        [Reactive] public string CoverageOutputLocation { get; set; }

        // ReSharper disable once UnusedMethodReturnValue.Local
        private ValidationHelper EmptyFieldValidation(Expression<Func<FormViewModel, string>> func,
                                                      string fieldName) =>
            this.ValidationRule(func, s => !string.IsNullOrWhiteSpace(s), $"{fieldName} should not be empty");
    }
}
