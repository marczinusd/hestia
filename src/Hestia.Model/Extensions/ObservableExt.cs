using System;
using System.Diagnostics.CodeAnalysis;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace Hestia.Model.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class ObservableExt
    {
        public static IObservable<T> CreateSingle<T>(Func<T> valueFactory)
        {
            return Observable.Create<T>(observer =>
            {
                try
                {
                    observer.OnNext(valueFactory());
                }
                catch (Exception e)
                {
                    observer.OnError(e);
                }

                observer.OnCompleted();

                return Disposable.Empty;
            });
        }
    }
}
