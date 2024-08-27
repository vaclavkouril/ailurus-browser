using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ailurus
{
    public class HistoryManager : IHistoryManager
    {
        private readonly string _historyFilePath = "history.json";
        private readonly List<HistoryItem> _historyItems = new List<HistoryItem>();

        public async Task AddToHistoryAsync(string url)
        {
            var historyItem = new HistoryItem(DateTime.Now, url);
            _historyItems.Add(historyItem);
            await SaveHistoryToFileAsync();
        }

        public async Task<IEnumerable<HistoryItem>> GetHistoryAsync()
        {
            await LoadHistoryFromFileAsync();
            // For the items to go from the most recent
            _historyItems.Reverse();
            return _historyItems;
        }

        public async Task ClearHistoryAsync()
        {
            _historyItems.Clear();
            if (File.Exists(_historyFilePath))
            {
                File.Delete(_historyFilePath);
            }
        }

        public async Task DeleteHistoryAsync()
        {
            await ClearHistoryAsync();
        }

        private async Task LoadHistoryFromFileAsync()
        {
            if (File.Exists(_historyFilePath))
            {
                var json = await File.ReadAllTextAsync(_historyFilePath);
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
            await File.WriteAllTextAsync(_historyFilePath, json);
        }
    }
}