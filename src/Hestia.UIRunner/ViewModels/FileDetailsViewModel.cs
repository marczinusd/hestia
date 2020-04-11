using System;
using Hestia.Model;
using ReactiveUI;

namespace Hestia.UIRunner.ViewModels
{
    public class FileDetailsViewModel : ViewModelBase
    {
        private readonly ObservableAsPropertyHelper<File> _fileObservableAsPropertyHelper;

        public FileDetailsViewModel(IObservable<File> observable)
        {
            _fileObservableAsPropertyHelper = observable.ToProperty(this, nameof(File));
        }

        public File File => _fileObservableAsPropertyHelper.Value;
    }
}
