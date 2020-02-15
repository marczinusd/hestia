using System.Collections.Generic;
using System.Linq;

namespace Hestia.Model.Wrappers
{
    public class GitCommands : IGitCommands
    {
        private readonly ICommandLineExecutor _commandLineExecutor;

        public GitCommands(ICommandLineExecutor commandLineExecutor)
        {
            _commandLineExecutor = commandLineExecutor;
        }

        public long NumberOfChangesForFile(string filePath)
        {
            var output = _commandLineExecutor.Execute(InterpolateCommandsForNumberOfChangesForFile(filePath));

            return ParseOutputForNumberOfChangesForFile(output);
        }

        // ReSharper disable once UnusedParameter.Local
        private IEnumerable<string> InterpolateCommandsForNumberOfChangesForFile(string filepath)
        {
            return Enumerable.Empty<string>();
        }

        // ReSharper disable once UnusedParameter.Local
        private long ParseOutputForNumberOfChangesForFile(string output)
        {
            return 0;
        }
    }
}
