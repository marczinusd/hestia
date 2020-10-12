﻿using System.Diagnostics.CodeAnalysis;
using Avalonia;
using Avalonia.Logging.Serilog;
using Avalonia.ReactiveUI;
using JetBrains.Annotations;

namespace Hestia.UIRunner
{
    [UsedImplicitly]
    [ExcludeFromCodeCoverage]
    public static class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        public static void Main(string[] args) => BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);

        // Avalonia configuration, don't remove; also used by visual designer.
        [UsedImplicitly]
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                         .UsePlatformDetect()
                         .LogToDebug()
                         .UseReactiveUI();
    }
}
