using Avalonia;
using System;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.ReactiveUI;
using Gecko;


namespace AilurusBrowserProject;

class Program
{
    class Program
    {   
        private static string FirefoxPath = "/usr/bin/firefox";

        [STAThread]
        public static void Main(string[] args)
        {
            Xpcom.Initialize(FirefoxPath);

            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }

        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                         .UsePlatformDetect()
                         .LogToTrace()
                         .UseReactiveUI();
    }
}
