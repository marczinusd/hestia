using System.Collections.Generic;

namespace Hestia.Model.Wrappers
{
    public interface ICommandLineExecutor
    {
        string Execute(IEnumerable<string> commandsToExecute);
    }
}
