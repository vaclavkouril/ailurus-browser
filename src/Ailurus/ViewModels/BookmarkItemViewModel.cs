using System;
using System.Reactive;
using System.Threading.Tasks;
using ReactiveUI;

namespace Ailurus.ViewModels;

public class BookmarkItemViewModel : ReactiveObject
{
    private readonly IBookmarkManager _bookmarkManager;
    private readonly MainWindowViewModel _mainWindowViewModel;

    public BookmarkItemViewModel(string url, string title, IBookmarkManager bookmarkManager, MainWindowViewModel mainWindowViewModel)
    {
        Url = url;
        Title = title;
        _bookmarkManager = bookmarkManager;
        _mainWindowViewModel = mainWindowViewModel;

        SelectBookmarkCommand = ReactiveCommand.Create(SelectBookmark);
        RemoveBookmarkCommand = ReactiveCommand.CreateFromTask(RemoveBookmarkAsync);
    }

    public string Url { get; }
    public string Title { get; }

    public ReactiveCommand<Unit, Unit> SelectBookmarkCommand { get; }
    public ReactiveCommand<Unit, Unit> RemoveBookmarkCommand { get; }

    private void SelectBookmark()
    {
        _mainWindowViewModel.EditableUrl = Url;
        _mainWindowViewModel.GoCommand.Execute().Subscribe();
    }

    private async Task RemoveBookmarkAsync()
    {
        await _bookmarkManager.RemoveBookmarkAsync(Url);
        await _mainWindowViewModel.LoadBookmarksAsync();
    }
}