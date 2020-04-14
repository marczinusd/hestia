using System.Threading.Tasks;

namespace Hestia.UIRunner.Services
{
    public interface IOpenFileDialogService
    {
        Task<string[]> OpenFileDialog();

        Task<string> OpenFolderDialog();
    }
}
