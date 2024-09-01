using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ailurus;

/// <summary>
/// Manages the browsing history, including adding, retrieving, and deleting history items.
/// </summary>
public class HistoryManager : IHistoryManager
{
    private const string HistoryFilePath = "history.json";
    private readonly List<HistoryItem> _historyItems = new();

    /// <summary>
    /// Adds a new URL to the browsing history.
    /// </summary>
    /// <param name="url">The URL to add to the history.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task AddToHistoryAsync(string url)
    {
        var historyItem = new HistoryItem(DateTime.Now, url);
        _historyItems.Add(historyItem);
        await SaveHistoryToFileAsync();
    }

    /// <summary>
    /// Retrieves the browsing history items in reverse chronological order.
    /// </summary>
    /// <returns>A <see cref="Task"/> containing an enumeration of history items.</returns>
    public async Task<IEnumerable<HistoryItem>> GetHistoryAsync()
    {
        await LoadHistoryFromFileAsync();
        _historyItems.Reverse(); // Order items from most recent to oldest
        return _historyItems;
    }

    /// <summary>
    /// Deletes all items from the browsing history.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task DeleteHistoryAsync()
    {
        await ClearHistoryAsync();
    }

    /// <summary>
    /// Clears the history list and deletes the history file if it exists.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    private async Task ClearHistoryAsync()
    {
        _historyItems.Clear();
        if (File.Exists(HistoryFilePath))
        {
            File.Delete(HistoryFilePath);
        }
        await Task.CompletedTask;
    }

    /// <summary>
    /// Loads the browsing history from the history file.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    private async Task LoadHistoryFromFileAsync()
    {
        if (File.Exists(HistoryFilePath))
        {
            var json = await File.ReadAllTextAsync(HistoryFilePath);
            var historyFromFile = JsonSerializer.Deserialize<List<HistoryItem>>(json);
            if (historyFromFile != null)
            {
                _historyItems.Clear();
                _historyItems.AddRange(historyFromFile);
            }
        }
    }

    /// <summary>
    /// Saves the current browsing history to the history file.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    private async Task SaveHistoryToFileAsync()
    {
        var json = JsonSerializer.Serialize(_historyItems);
        await File.WriteAllTextAsync(HistoryFilePath, json);
    }
}