using System;
using System.Linq;
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

        public static AppBuilder BuildAvaloniaApp(bool isAnonymousMode)
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace()
                .UseReactiveUI()
                .AfterSetup(app =>
                {
                    ((App)app.Instance).Configure(isAnonymousMode);
                });

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

    internal class AilurusCefApp : CefApp
    {
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
            commandLine.AppendSwitch("disable-software-rasterizer");  // Disable software fallback
            commandLine.AppendSwitch("ignore-gpu-blocklist");  // Ignore GPU blocklist
            commandLine.AppendSwitch("enable-native-gpu-memory-buffers");
            */
        }
    }
}
