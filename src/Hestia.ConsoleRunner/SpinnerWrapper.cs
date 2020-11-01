using System;
using System.Diagnostics.CodeAnalysis;
using Kurukuru;

namespace Hestia.ConsoleRunner
{
    [ExcludeFromCodeCoverage]
    public class SpinnerWrapper : ISpinner
    {
        public void Start(string message, Action actionToRun) => Spinner.Start(message, actionToRun);
    }
}
