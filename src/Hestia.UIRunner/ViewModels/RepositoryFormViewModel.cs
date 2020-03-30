using System.IO;
using Hestia.Model.Wrappers;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Abstractions;
using ReactiveUI.Validation.Contexts;
using ReactiveUI.Validation.Extensions;

namespace Hestia.UIRunner.ViewModels
{
    public class RepositoryFormViewModel : ViewModelBase, IValidatableViewModel
    {
        public RepositoryFormViewModel(IDiskIOWrapper ioWrapper)
        {
            this.ValidationRule(vm => vm.RepositoryPath,
                                ioWrapper.DirectoryExists,
                                "Directory does not exist.");

            this.ValidationRule(vm => vm.RepositoryPath,
                                path => ioWrapper.DirectoryExists(Path.Join(path, ".git")),
                                "Directory is not a git repository");
        }

        public ValidationContext ValidationContext { get; } = new ValidationContext();

        [Reactive] public string RepositoryPath { get; set; }
    }
}
