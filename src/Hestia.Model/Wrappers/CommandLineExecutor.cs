using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using SimpleExec;

namespace Hestia.Model.Wrappers
{
    [ExcludeFromCodeCoverage]
    public class CommandLineExecutor : ICommandLineExecutor
    {
        private readonly bool _noEcho;

        public CommandLineExecutor(bool noEcho = true)
        {
            _noEcho = noEcho;
        }

        public string[] Execute(string commandToExecute, string args, string workingDirectory) =>
            Command.Read(commandToExecute,
                         args,
                         workingDirectory,
                         _noEcho)
                   .Split(Environment.NewLine);

        public string ExecuteNoSplit(string commandToExecute, string args, string workingDirectory) =>
            Command.Read(commandToExecute,
                         args,
                         workingDirectory,
                         _noEcho);

        public async Task<string[]> ExecuteAsync(string commandToExecute, string args, string workingDirectory) =>
            (await Command.ReadAsync(commandToExecute,
                                     args,
                                     workingDirectory,
                                     _noEcho)).Split(Environment.NewLine);

        public async Task<string> ExecuteAsyncNoSplit(string commandToExecute, string args, string workingDirectory) =>
            await Command.ReadAsync(commandToExecute,
                                    args,
                                    workingDirectory,
                                    _noEcho);
    }
}
