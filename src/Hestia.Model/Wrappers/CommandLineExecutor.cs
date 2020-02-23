using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SimpleExec;

namespace Hestia.Model.Wrappers
{
    public class CommandLineExecutor : ICommandLineExecutor
    {
        public string[] Execute(IEnumerable<string> commandsToExecute) =>
            ExecuteAsync(commandsToExecute)
                .Result;

        public string[] Execute(string commandToExecute) =>
            ExecuteAsync(commandToExecute)
                .Result;

        public string ExecuteNoSplit(string commandToExecute) =>
            ExecuteAsyncNoSplit(commandToExecute)
                .Result;

        public async Task<string[]> ExecuteAsync(string commandToExecute) =>
            (await Command.ReadAsync(commandToExecute)).Split(Environment.NewLine);

        public async Task<string> ExecuteAsyncNoSplit(string commandToExecute) =>
            await Command.ReadAsync(commandToExecute);

        public async Task<string[]> ExecuteAsync(IEnumerable<string> commandsToExecute) =>
            await ExecuteAsync(string.Join(" && ", commandsToExecute));
    }
}
