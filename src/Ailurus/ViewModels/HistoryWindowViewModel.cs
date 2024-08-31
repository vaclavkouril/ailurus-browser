using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;

namespace Ailurus.ViewModels;

public class HistoryWindowViewModel : ReactiveObject
{
    private readonly IHistoryManager _historyManager;

    public ObservableCollection<HistoryItemViewModel> HistoryItems { get; } = [];

    public ReactiveCommand<Unit, Unit> ClearHistoryCommand { get; }

    public HistoryWindowViewModel(IHistoryManager historyManager)
    {
        _historyManager = historyManager;

        ClearHistoryCommand = ReactiveCommand.CreateFromTask(ClearHistoryAsync);

        LoadHistoryAsync().ConfigureAwait(false);
    }

    private async Task LoadHistoryAsync()
    {
        var history = await _historyManager.GetHistoryAsync();
        foreach (var item in history)
        {
            HistoryItems.Add(new HistoryItemViewModel(item.Timestamp, item.Url));
        }
    }

    private async Task ClearHistoryAsync()
    {
        await _historyManager.DeleteHistoryAsync();
        HistoryItems.Clear();
    }
}