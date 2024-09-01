using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Ailurus.ViewModels;
using System.Threading.Tasks;
using Xilium.CefGlue;

namespace Ailurus;

public class App : Application
{
    private bool _isAnonymousMode;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public void Configure(bool isAnonymousMode)
    {
        _isAnonymousMode = isAnonymousMode;
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

            IBookmarkManager bookmarkManager = _isAnonymousMode
                ? new AnonymousBookmarkManager()
                : new BookmarkManager();

            IConfigurationManager configurationManager = new ConfigurationManager();

            var mainWindowViewModel = new MainWindowViewModel(sessionManager,
                historyManager,
                bookmarkManager,
                configurationManager);

            desktop.MainWindow = new MainWindow(mainWindowViewModel)
            {
                Title = _isAnonymousMode ? "Anonymous-Ailurus" : "Ailurus Web Browser"
            };

            desktop.Exit += async (_, _) =>
            {
                await mainWindowViewModel.SaveSessionAsync();
                await ShutdownApplicationAsync();
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private static async Task ShutdownApplicationAsync()
    {
        // Properly shutdown CefGlue
        CefRuntime.Shutdown();
        await Task.CompletedTask;
    }
}