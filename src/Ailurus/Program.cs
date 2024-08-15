using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.ReactiveUI;
using Xilium.CefGlue;
using Xilium.CefGlue.Common;
using Xilium.CefGlue.Avalonia;

namespace Ailurus
{
    class Program
    {
        
        [STAThread]
        public static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Starting application...");

                InitializeCef(args);

                Console.WriteLine("CEF Initialized, starting Avalonia...");

                BuildAvaloniaApp()
                    .StartWithClassicDesktopLifetime(args);

                Console.WriteLine("Avalonia started, entering main loop...");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occurred: " + ex);
            }
            finally
            {
                Console.WriteLine("Shutting down CEF...");
                CefRuntime.Shutdown();
                Console.WriteLine("CEF Shutdown complete.");
            }
        }


        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace()
                .UseReactiveUI();

        private static void InitializeCef(string[] args)
        {
            var cefBinaryPath = System.IO.Path.Combine(AppContext.BaseDirectory, "cef_binary");

            var cefSettings = new CefSettings
            {
                BrowserSubprocessPath = System.IO.Path.Combine(cefBinaryPath, "chrome-sandbox"),
                LocalesDirPath = System.IO.Path.Combine(cefBinaryPath, "locales"),
                ResourcesDirPath = cefBinaryPath,
                LogSeverity = CefLogSeverity.Verbose,
                LogFile = "cef.log",
                CachePath = System.IO.Path.Combine(cefBinaryPath, "cache") 
            };

            CefRuntimeLoader.Initialize();
            Console.WriteLine("Intialize Start...");
            CefRuntime.Initialize(new CefMainArgs(args), cefSettings, new MyCefApp(), IntPtr.Zero);
            Console.WriteLine("...Intialize End");
        }
    }

    internal class MyCefApp : CefApp
    {
        protected override void OnBeforeCommandLineProcessing(string processType, CefCommandLine commandLine)
        {
            commandLine.AppendSwitch("no-sandbox");
            commandLine.AppendSwitch("disable-setuid-sandbox");
            commandLine.AppendSwitch("disable-gpu");
            commandLine.AppendSwitch("disable-software-rasterizer");
        }
    }
}
