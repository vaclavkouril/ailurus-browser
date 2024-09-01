using System;

namespace Ailurus;

/// <summary>
/// Represents a history item with a timestamp and URL.
/// </summary>
/// <param name="timestamp">The timestamp of when the history item was recorded.</param>
/// <param name="url">The URL of the history item.</param>
public class HistoryItem(DateTime timestamp, string url)
{
    public DateTime Timestamp { get; } = timestamp;
    public string Url { get; } = url;

    /// <summary>
    /// Returns a string representation of the history item.
    /// </summary>
    /// <returns>A string in the format "Timestamp - URL".</returns>
    public override string ToString()
    {
        return $"{Timestamp:G} - {Url}";
    }
}