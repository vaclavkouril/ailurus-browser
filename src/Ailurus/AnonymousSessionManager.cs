using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ailurus.ViewModels;

namespace Ailurus
{
    /// <summary>
    /// A session manager that operates in anonymous mode, meaning no session data is saved or loaded.
    /// </summary>
    public class AnonymousSessionManager : ISessionManager
    {
        /// <summary>
        /// Does not save the current session as anonymous mode does not persist session data.
        /// </summary>
        /// <param name="tabs">The collection of <see cref="BrowserTabViewModel"/> instances representing the open tabs.</param>
        /// <returns>A <see cref="Task"/> representing the completed operation.</returns>
        public Task SaveSessionAsync(IEnumerable<BrowserTabViewModel> tabs)
        {
            // No saving in anonymous mode
            return Task.CompletedTask;
        }

        /// <summary>
        /// Does not load any session as anonymous mode does not persist session data.
        /// </summary>
        /// <param name="window">The <see cref="MainWindowViewModel"/> where the tabs would be restored.</param>
        /// <returns>A <see cref="Task"/> containing an empty collection of <see cref="BrowserTabViewModel"/> instances.</returns>
        public Task<IEnumerable<BrowserTabViewModel>> LoadSessionAsync(MainWindowViewModel window)
        {
            // No session to load in anonymous mode
            return Task.FromResult(Enumerable.Empty<BrowserTabViewModel>());
        }
    }
}