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
        }

        public ValidationContext ValidationContext { get; } = new ValidationContext();

        [Reactive] public string RepositoryPath { get; set; }
    }
}
