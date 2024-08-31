using System.Collections.Generic;
using System.Threading.Tasks;
using Ailurus.ViewModels;

namespace Ailurus;

/// <summary>
/// Interface representing session management operations for the browser.
/// </summary>
public interface ISessionManager
{
    /// <summary>
    /// Saves the current session by persisting information about open tabs.
    /// </summary>
    /// <param name="tabs">The collection of <see cref="BrowserTabViewModel"/> instances representing the open tabs.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous save operation.</returns>
    Task SaveSessionAsync(IEnumerable<BrowserTabViewModel> tabs);

    /// <summary>
    /// Loads a saved session by restoring tabs from previously persisted information.
    /// </summary>
    /// <param name="window">The <see cref="MainWindowViewModel"/> where the tabs will be restored.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous load operation, with a collection of restored <see cref="BrowserTabViewModel"/> instances.</returns>
    Task<IEnumerable<BrowserTabViewModel>> LoadSessionAsync(MainWindowViewModel window);
}