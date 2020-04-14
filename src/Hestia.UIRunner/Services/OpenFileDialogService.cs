using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Avalonia.Controls;

namespace Hestia.UIRunner.Services
{
    // excluded from coverage as it's basically a wrapper around avalonia's file dialog
    [ExcludeFromCodeCoverage]
    public class OpenFileDialogService : IOpenFileDialogService
    {
        private readonly Func<Window> _window;

        public OpenFileDialogService(Func<Window> window)
        {
            _window = window;
        }

        public async Task<string[]> OpenFileDialog()
        {
            var dialog = new OpenFileDialog();

            return await dialog.ShowAsync(_window());
        }

        public async Task<string> OpenFolderDialog()
        {
            var dialog = new OpenFolderDialog();

            return await dialog.ShowAsync(_window());
        }
    }
}
