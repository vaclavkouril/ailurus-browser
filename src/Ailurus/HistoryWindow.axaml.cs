using Avalonia.Controls;
using Ailurus.ViewModels;

namespace Ailurus;

public partial class HistoryWindow : Window
{
    public HistoryWindow(IHistoryManager historyManager)
    {
        InitializeComponent();
        DataContext = new HistoryWindowViewModel(historyManager);
    }
}