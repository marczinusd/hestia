using System;
using System.Diagnostics.CodeAnalysis;
using ShellProgressBar;

namespace Hestia.ConsoleRunner
{
    [ExcludeFromCodeCoverage]
    public class ProgressBarFactory : IProgressBarFactory
    {
        public IDisposable CreateProgressBar(IObservable<int> progressSubject, int total)
        {
            var options = new ProgressBarOptions { ProgressBarOnBottom = true };
            var progressBar = new ProgressBar(total, "Processing git stats for all files", options);
            progressSubject.Subscribe(val =>
            {
                progressBar.Tick($"File {val} of {total}");
            }); // don't try this at home

            return progressBar;
        }
    }
}
