using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
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
            var progress = 0;
            progressSubject.Subscribe(_ =>
            {
                progressBar.Tick($"Processed {Interlocked.Add(ref progress, 1)} of {total} files");
            }); // don't try this at home

            return progressBar;
        }
    }
}
