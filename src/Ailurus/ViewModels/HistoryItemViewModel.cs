using System;

namespace Ailurus.ViewModels
{
    /// <summary>
    /// ViewModel representing a single history item.
    /// </summary>
    public class HistoryItemViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HistoryItemViewModel"/> class.
        /// </summary>
        /// <param name="timestamp">The timestamp of the history item.</param>
        /// <param name="url">The URL of the history item.</param>
        public HistoryItemViewModel(DateTime timestamp, string url)
        {
            Timestamp = timestamp;
            Url = url;
        }

        /// <summary>
        /// Gets the timestamp of the history item.
        /// </summary>
        public DateTime Timestamp { get; }

        /// <summary>
        /// Gets the URL of the history item.
        /// </summary>
        public string Url { get; }
    }
}