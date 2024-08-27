using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Ailurus.ViewModels;

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

                var mainWindowViewModel = new MainWindowViewModel(sessionManager);

                desktop.MainWindow = new MainWindow(mainWindowViewModel);

                desktop.Exit += async (_, __) => await mainWindowViewModel.SaveSessionAsync();
            }

            base.OnFrameworkInitializationCompleted();
        }

        public void EnableAnonymousMode()
        {
            _isAnonymousMode = true;
        }
    }

}