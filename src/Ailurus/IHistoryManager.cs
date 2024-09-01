using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ailurus;

/// <summary>
/// Interface for managing the browsing history within the browser application.
/// </summary>
public interface IHistoryManager
{
    /// <summary>
    /// Adds a new entry to the browsing history with the specified URL.
    /// </summary>
    /// <param name="url">The URL to add to the history.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task AddToHistoryAsync(string url);

    /// <summary>
    /// Retrieves the browsing history.
    /// </summary>
    /// <returns>A <see cref="Task"/> containing a collection of <see cref="HistoryItem"/> representing the browsing history.</returns>
    Task<IEnumerable<HistoryItem>> GetHistoryAsync();

    /// <summary>
    /// Deletes all entries from the browsing history.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task DeleteHistoryAsync();
}