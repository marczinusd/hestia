﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using SimpleExec;

namespace Hestia.Model.Wrappers
{
    public enum ExecutorEchoMode
    {
        /// <summary>
        ///     command results and the command executed will be echoed.
        /// </summary>
        Echo,

        /// <summary>
        ///     only command results will get echoed.
        /// </summary>
        NoEcho
    }

    [ExcludeFromCodeCoverage]
    public class CommandLineExecutor : ICommandLineExecutor
    {
        private readonly bool _noEcho;

        public CommandLineExecutor(ExecutorEchoMode echoMode) => _noEcho = echoMode == ExecutorEchoMode.NoEcho;

        public string[] Execute(string commandToExecute, string args, string workingDirectory)
        {
            var result = Command.Read(commandToExecute,
                                      args,
                                      workingDirectory,
                                      _noEcho);

            if (result.Contains(Environment.NewLine))
            {
                // Ideally only this should suffice, but due to some weirdness with SimpleExec this only works on linux
                return result.Split(Environment.NewLine);
            }

            return result.Split('\n');
        }

        public string ExecuteNoSplit(string commandToExecute, string args, string workingDirectory) =>
            Command.Read(commandToExecute,
                         args,
                         workingDirectory,
                         _noEcho);

        public async Task<string[]> ExecuteAsync(string commandToExecute, string args, string workingDirectory) =>
            (await Command.ReadAsync(commandToExecute,
                                     args,
                                     workingDirectory,
                                     _noEcho)
                          .ConfigureAwait(false)).Split(Environment.NewLine);

        public async Task<string> ExecuteAsyncNoSplit(string commandToExecute, string args, string workingDirectory) =>
            await Command.ReadAsync(commandToExecute,
                                    args,
                                    workingDirectory,
                                    _noEcho)
                         .ConfigureAwait(false);
    }
}
