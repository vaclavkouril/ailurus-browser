using System;
using System.Reactive;
using System.Threading.Tasks;
using ReactiveUI;

namespace Ailurus.ViewModels
{
    /// <summary>
    /// ViewModel representing a single bookmark item.
    /// </summary>
    public class BookmarkItemViewModel : ReactiveObject
    {
        private readonly IBookmarkManager _bookmarkManager;
        private readonly MainWindowViewModel _mainWindowViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookmarkItemViewModel"/> class.
        /// </summary>
        /// <param name="url">The URL of the bookmark.</param>
        /// <param name="title">The title of the bookmark.</param>
        /// <param name="bookmarkManager">The bookmark manager.</param>
        /// <param name="mainWindowViewModel">The main window view model.</param>
        public BookmarkItemViewModel(string url, string title, IBookmarkManager bookmarkManager, MainWindowViewModel mainWindowViewModel)
        {
            Url = url;
            Title = title;
            _bookmarkManager = bookmarkManager;
            _mainWindowViewModel = mainWindowViewModel;

            SelectBookmarkCommand = ReactiveCommand.Create(SelectBookmark);
            RemoveBookmarkCommand = ReactiveCommand.CreateFromTask(RemoveBookmarkAsync);
        }

        /// <summary>
        /// Gets the URL of the bookmark.
        /// </summary>
        public string Url { get; }

        /// <summary>
        /// Gets the title of the bookmark.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Command to select the bookmark, navigating to its URL.
        /// </summary>
        public ReactiveCommand<Unit, Unit> SelectBookmarkCommand { get; }

        /// <summary>
        /// Command to remove the bookmark.
        /// </summary>
        public ReactiveCommand<Unit, Unit> RemoveBookmarkCommand { get; }

        /// <summary>
        /// Selects the bookmark by setting the URL in the main window and executing the Go command.
        /// </summary>
        private void SelectBookmark()
        {
            _mainWindowViewModel.EditableUrl = Url;
            _mainWindowViewModel.GoCommand.Execute().Subscribe();
        }

        /// <summary>
        /// Removes the bookmark asynchronously and reloads the bookmarks list in the main window.
        /// </summary>
        private async Task RemoveBookmarkAsync()
        {
            await _bookmarkManager.RemoveBookmarkAsync(Url);
            await _mainWindowViewModel.LoadBookmarksAsync();
        }
    }
}
