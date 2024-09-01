using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Ailurus.ViewModels;
using System.Threading.Tasks;
using Xilium.CefGlue;

namespace Ailurus;

/// <summary>
/// Represents the main application class for Ailurus. It initializes and configures the application, 
/// including setting up anonymous mode if specified.
/// </summary>
public class App : Application
{
    private bool _isAnonymousMode;

    /// <summary>
    /// Initializes the application by loading the XAML resources.
    /// </summary>
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    /// <summary>
    /// Configures the application mode (anonymous or normal).
    /// </summary>
    /// <param name="isAnonymousMode">Indicates whether the application should run in anonymous mode.</param>
    public void Configure(bool isAnonymousMode)
    {
        _isAnonymousMode = isAnonymousMode;
    }

    /// <summary>
    /// Called when the application framework initialization is completed. 
    /// Sets up the main window and configures session, history, and bookmark managers based on the application mode.
    /// </summary>
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

    /// <summary>
    /// Shuts down the application, including any resources used by the CefGlue browser.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the shutdown process.</returns>
    private static async Task ShutdownApplicationAsync()
    {
        CefRuntime.Shutdown();
        await Task.CompletedTask;
    }
}