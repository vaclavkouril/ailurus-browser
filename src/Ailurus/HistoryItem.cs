using System;

namespace Ailurus
{
    public class HistoryItem
    {
        public DateTime Timestamp { get; set; }
        public string Url { get; set; } = string.Empty;

        public HistoryItem(DateTime timestamp, string url)
        {
            Timestamp = timestamp;
            Url = url;
        }

        public override string ToString()
        {
            return $"{Timestamp:G} - {Url}";
        }
    }
}