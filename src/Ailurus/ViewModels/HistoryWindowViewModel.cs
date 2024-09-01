using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;

namespace Ailurus.ViewModels
{
    /// <summary>
    /// ViewModel for managing the history window.
    /// </summary>
    public class HistoryWindowViewModel : ReactiveObject
    {
        private readonly IHistoryManager _historyManager;

        /// <summary>
        /// Gets the collection of history items.
        /// </summary>
        public ObservableCollection<HistoryItemViewModel> HistoryItems { get; } = new ObservableCollection<HistoryItemViewModel>();

        /// <summary>
        /// Command to clear the history.
        /// </summary>
        public ReactiveCommand<Unit, Unit> ClearHistoryCommand { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HistoryWindowViewModel"/> class.
        /// </summary>
        /// <param name="historyManager">The history manager.</param>
        public HistoryWindowViewModel(IHistoryManager historyManager)
        {
            _historyManager = historyManager;

            ClearHistoryCommand = ReactiveCommand.CreateFromTask(ClearHistoryAsync);

            LoadHistoryAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Loads the browsing history asynchronously and populates the HistoryItems collection.
        /// </summary>
        private async Task LoadHistoryAsync()
        {
            var history = await _historyManager.GetHistoryAsync();
            foreach (var item in history)
            {
                HistoryItems.Add(new HistoryItemViewModel(item.Timestamp, item.Url));
            }
        }

        /// <summary>
        /// Clears the browsing history asynchronously.
        /// </summary>
        private async Task ClearHistoryAsync()
        {
            await _historyManager.DeleteHistoryAsync();
            HistoryItems.Clear();
        }
    }
}