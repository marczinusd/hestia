using System;

namespace Hestia.ConsoleRunner
{
    public interface ISpinner
    {
        public void Start(string message, Action actionToRun);
    }
}
