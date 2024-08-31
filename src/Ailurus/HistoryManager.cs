using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ailurus;

public class HistoryManager : IHistoryManager
{
    private const string HistoryFilePath = "history.json";
    private readonly List<HistoryItem> _historyItems = [];

    public async Task AddToHistoryAsync(string url)
    {
        var historyItem = new HistoryItem(DateTime.Now, url);
        _historyItems.Add(historyItem);
        await SaveHistoryToFileAsync();
    }

    public async Task<IEnumerable<HistoryItem>> GetHistoryAsync()
    {
        await LoadHistoryFromFileAsync();
        // For the items to be ordered from the most recent
        _historyItems.Reverse();
        return _historyItems;
    }

    private async Task ClearHistoryAsync()
    {
        _historyItems.Clear();
        if (File.Exists(HistoryFilePath))
        {
            File.Delete(HistoryFilePath);
        }
    }

    public async Task DeleteHistoryAsync()
    {
        await ClearHistoryAsync();
    }

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

    private async Task SaveHistoryToFileAsync()
    {
        var json = JsonSerializer.Serialize(_historyItems);
        await File.WriteAllTextAsync(HistoryFilePath, json);
    }
}