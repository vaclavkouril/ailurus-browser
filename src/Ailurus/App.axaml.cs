using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

namespace Ailurus
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        
        public override void OnFrameworkInitializationCompleted()
        {
            try
            {
                Console.WriteLine("Framework Initialization started.");

                if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                {
                    Console.WriteLine("Setting up MainWindow.");
                    desktop.MainWindow = new MainWindow();
                }

                base.OnFrameworkInitializationCompleted();
                Console.WriteLine("Framework Initialization completed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception during Avalonia Framework Initialization: " + ex);
                throw;
            }
        }
        }
}
