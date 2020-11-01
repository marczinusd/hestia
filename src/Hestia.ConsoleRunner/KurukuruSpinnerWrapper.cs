using System;
using System.Diagnostics.CodeAnalysis;
using Kurukuru;

namespace Hestia.ConsoleRunner
{
    [ExcludeFromCodeCoverage]
    public class KurukuruSpinnerWrapper : ISpinner
    {
        public void Start(string message, Action actionToRun) => Spinner.Start(message, actionToRun);
    }
}
