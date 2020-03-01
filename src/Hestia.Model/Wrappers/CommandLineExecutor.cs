﻿using System;
using System.Threading.Tasks;
using SimpleExec;

namespace Hestia.Model.Wrappers
{
    public class CommandLineExecutor : ICommandLineExecutor
    {
        private readonly bool _noEcho;

        public CommandLineExecutor(bool noEcho = true)
        {
            _noEcho = noEcho;
        }

        public string[] Execute(string commandToExecute, string args, string workingDirectory) =>
            ExecuteAsync(commandToExecute, args, workingDirectory).Result;

        public string ExecuteNoSplit(string commandToExecute, string args, string workingDirectory) =>
            ExecuteAsyncNoSplit(commandToExecute, args, workingDirectory).Result;

        public async Task<string[]> ExecuteAsync(string commandToExecute, string args, string workingDirectory) =>
            (await Command.ReadAsync(commandToExecute, args, workingDirectory, _noEcho)).Split(Environment.NewLine);

        public async Task<string> ExecuteAsyncNoSplit(string commandToExecute, string args, string workingDirectory) =>
            await Command.ReadAsync(commandToExecute, args, workingDirectory, _noEcho);
    }
}
