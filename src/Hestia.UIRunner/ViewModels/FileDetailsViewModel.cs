using System;
using Hestia.Model.Interfaces;
using ReactiveUI;

namespace Hestia.UIRunner.ViewModels
{
    public class FileDetailsViewModel : ReactiveObject
    {
        private readonly ObservableAsPropertyHelper<IFile> _fileObservableAsPropertyHelper;

        public FileDetailsViewModel(IObservable<IFile> observable) =>
            _fileObservableAsPropertyHelper = observable.ToProperty(this, nameof(File));

        public IFile File => _fileObservableAsPropertyHelper.Value;
    }
}
