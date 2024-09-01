using Avalonia.Controls;
using Ailurus.ViewModels;

namespace Ailurus;

/// <summary>
/// Represents a window that displays the browsing history.
/// </summary>
public partial class HistoryWindow : Window
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HistoryWindow"/> class.
    /// </summary>
    /// <param name="historyManager">The history manager used to manage browsing history.</param>
    public HistoryWindow(IHistoryManager historyManager)
    {
        InitializeComponent();
        DataContext = new HistoryWindowViewModel(historyManager);
    }
}