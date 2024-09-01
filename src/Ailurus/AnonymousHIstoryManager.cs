using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ailurus;


/// <summary>
/// Manages browser history in anonymous mode, where no history data is persisted or retrieved.
/// </summary>
public class AnonymousHistoryManager : IHistoryManager
{
    /// <summary>
    /// Adds a URL to the browsing history asynchronously. In anonymous mode, this does nothing.
    /// </summary>
    /// <param name="url">The URL to add to the history.</param>
    /// <returns>A <see cref="Task"/> representing the completed operation.</returns>
    public Task AddToHistoryAsync(string url)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Retrieves the browsing history asynchronously. In anonymous mode, this returns an empty collection.
    /// </summary>
    /// <returns>A <see cref="Task"/> containing an empty enumeration of <see cref="HistoryItem"/>.</returns>
    public Task<IEnumerable<HistoryItem>> GetHistoryAsync()
    {
        return Task.FromResult<IEnumerable<HistoryItem>>(new List<HistoryItem>());
    }

    /// <summary>
    /// Deletes all browsing history asynchronously. In anonymous mode, this does nothing.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the completed operation.</returns>
    public Task DeleteHistoryAsync()
    {
        return Task.CompletedTask;
    }
}