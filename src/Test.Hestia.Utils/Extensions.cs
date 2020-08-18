using System.Diagnostics.CodeAnalysis;
using System.Reactive;
using Microsoft.Reactive.Testing;

namespace Test.Hestia.Utils
{
    [ExcludeFromCodeCoverage]
    public static class Extensions
    {
        public static Recorded<Notification<T>> AsNotification<T>(this T val) =>
            new Recorded<Notification<T>>(0, Notification.CreateOnNext(val));
    }
}
