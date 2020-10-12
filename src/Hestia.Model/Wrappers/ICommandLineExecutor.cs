using System.Threading.Tasks;

namespace Hestia.Model.Wrappers
{
    public interface ICommandLineExecutor
    {
        string[] Execute(string commandToExecute, string args, string workingDirectory);

        string ExecuteNoSplit(string commandToExecute, string args, string workingDirectory);

        Task<string[]> ExecuteAsync(string commandToExecute, string args, string workingDirectory);

        Task<string> ExecuteAsyncNoSplit(string commandToExecute, string args, string workingDirectory);
    }
}
