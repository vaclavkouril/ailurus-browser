using System;

namespace Ailurus.ViewModels;

public class HistoryItemViewModel
{
    public HistoryItemViewModel(DateTime timestamp, string url)
    {
        Timestamp = timestamp;
        Url = url;
    }

    public DateTime Timestamp { get; }
    public string Url { get; }
}