using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Ailurus.ViewModels;
using System.Threading.Tasks;
using Xilium.CefGlue;

namespace Ailurus
{
    public class App : Application
    {
        private bool _isAnonymousMode;

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                ISessionManager sessionManager = _isAnonymousMode
                    ? new AnonymousSessionManager()
                    : new SessionManager();

                IHistoryManager historyManager = _isAnonymousMode
                    ? new AnonymousHistoryManager()
                    : new HistoryManager();

                var mainWindowViewModel = new MainWindowViewModel(sessionManager, historyManager);

                desktop.MainWindow = new MainWindow(mainWindowViewModel);

                desktop.Exit += async (_, __) =>
                {
                    await mainWindowViewModel.SaveSessionAsync();
                    await ShutdownApplicationAsync();
                };
            }

            base.OnFrameworkInitializationCompleted();
        }

        public void EnableAnonymousMode()
        {
            _isAnonymousMode = true;
        }

        private async Task ShutdownApplicationAsync()
        {
            // Properly shutdown CefGlue
            CefRuntime.Shutdown();
            await Task.CompletedTask;
        }
    }
}