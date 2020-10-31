using System;

namespace Hestia.ConsoleRunner
{
    public interface IProgressBarFactory
    {
        IDisposable CreateProgressBar(IObservable<int> progressSubject, int total);
    }
}
