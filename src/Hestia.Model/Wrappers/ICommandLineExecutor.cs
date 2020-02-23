using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hestia.Model.Wrappers
{
    public interface ICommandLineExecutor
    {
        string[] Execute(IEnumerable<string> commandsToExecute);

        string[] Execute(string commandToExecute);

        string ExecuteNoSplit(string commandToExecute);

        Task<string[]> ExecuteAsync(string commandToExecute);

        Task<string> ExecuteAsyncNoSplit(string commandToExecute);

        Task<string[]> ExecuteAsync(IEnumerable<string> commandsToExecute);
    }
}
