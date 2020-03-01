using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SimpleExec;

namespace Hestia.Model.Wrappers
{
    public class CommandLineExecutor : ICommandLineExecutor
    {
        private readonly bool _echoDetails;

        public CommandLineExecutor(bool echoDetails = false)
        {
            _echoDetails = echoDetails;
        }

        public string[] Execute(string commandToExecute, string args, string workingDirectory) =>
            ExecuteAsync(commandToExecute, args, workingDirectory).Result;

        public string ExecuteNoSplit(string commandToExecute, string args, string workingDirectory) =>
            ExecuteAsyncNoSplit(commandToExecute, args, workingDirectory).Result;

        public async Task<string[]> ExecuteAsync(string commandToExecute, string args, string workingDirectory) =>
            (await Command.ReadAsync(commandToExecute, args, workingDirectory, _echoDetails)).Split(Environment.NewLine);

        public async Task<string> ExecuteAsyncNoSplit(string commandToExecute, string args, string workingDirectory) =>
            await Command.ReadAsync(commandToExecute, args, workingDirectory, _echoDetails);
    }
}
