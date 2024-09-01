using System;
using System.Linq;
using Avalonia;
using Avalonia.ReactiveUI;
using Xilium.CefGlue;
using Xilium.CefGlue.Common;

namespace Ailurus;

/// <summary>
/// The entry point of the application, responsible for initializing and starting the application.
/// </summary>
internal static class Program
{
    /// <summary>
    /// The main entry point method for the application.
    /// </summary>
    /// <param name="args">The command-line arguments.</param>
    [STAThread]
    public static void Main(string[] args)
    {
        try
        {
            bool isAnonymousMode = args.Contains("--anonymous");

            InitializeCef(args, isAnonymousMode);

            BuildAvaloniaApp(isAnonymousMode)
                .StartWithClassicDesktopLifetime(args);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception occurred: " + ex);
        }
        finally
        {
            CefRuntime.Shutdown();
        }
    }

    /// <summary>
    /// Builds the Avalonia application with the specified configuration.
    /// </summary>
    /// <param name="isAnonymousMode">Indicates if the application should run in anonymous mode.</param>
    /// <returns>An <see cref="AppBuilder"/> instance configured for the application.</returns>
    public static AppBuilder BuildAvaloniaApp(bool isAnonymousMode)
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .LogToTrace()
            .UseReactiveUI()
            .AfterSetup(app =>
            {
                ((App)app.Instance).Configure(isAnonymousMode);
            });

    /// <summary>
    /// Initializes the Chromium Embedded Framework (CEF) with the specified settings.
    /// </summary>
    /// <param name="args">The command-line arguments.</param>
    /// <param name="isAnonymousMode">Indicates if the application should run in anonymous mode.</param>
    private static void InitializeCef(string[] args, bool isAnonymousMode)
    {
        var cefBinaryPath = System.IO.Path.Combine(AppContext.BaseDirectory, "cef_binary");

        var cefSettings = new CefSettings
        {
            LocalesDirPath = System.IO.Path.Combine(cefBinaryPath, "locales"),
            ResourcesDirPath = cefBinaryPath,
            CachePath = isAnonymousMode 
                ? System.IO.Path.Combine(cefBinaryPath, "anonymous_cache")
                : System.IO.Path.Combine(cefBinaryPath, "cache"),
            RootCachePath = System.IO.Path.Combine(cefBinaryPath, "root_cache"),
            Locale = "en-US",
            MultiThreadedMessageLoop = true,
            LogSeverity = CefLogSeverity.Verbose,
            LogFile = "cef.log"
        };

        CefRuntimeLoader.Initialize();
        CefRuntime.Initialize(new CefMainArgs(args), cefSettings, new AilurusCefApp(), IntPtr.Zero);
    }
}


/// <summary>
/// Custom CEF application class for handling CEF-specific configurations and settings.
/// </summary>
internal class AilurusCefApp : CefApp
{
    /// <summary>
    /// Configures command-line arguments before CEF processes them.
    /// </summary>
    /// <param name="processType">The type of process being started by CEF.</param>
    /// <param name="commandLine">The command-line instance to modify.</param>
    protected override void OnBeforeCommandLineProcessing(string processType, CefCommandLine commandLine)
    {
        /*
        commandLine.AppendSwitch("no-sandbox");
        commandLine.AppendSwitch("disable-gpu");
        commandLine.AppendSwitch("disable-software-rasterizer");
        commandLine.AppendSwitch("disable-gpu-compositing");

        commandLine.AppendSwitch("enable-gpu");
        commandLine.AppendSwitch("enable-gpu-rasterization");
        commandLine.AppendSwitch("enable-zero-copy");
        commandLine.AppendSwitch("disable-software-rasterizer");
        commandLine.AppendSwitch("ignore-gpu-blocklist");
        commandLine.AppendSwitch("enable-native-gpu-memory-buffers");
        */
    }
}