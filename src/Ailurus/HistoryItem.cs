using System;

namespace Ailurus;

public class HistoryItem(DateTime timestamp, string url)
{
    public DateTime Timestamp { get; } = timestamp;
    public string Url { get; } = url;

    public override string ToString()
    {
        return $"{Timestamp:G} - {Url}";
    }
}