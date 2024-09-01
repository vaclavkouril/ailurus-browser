using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Ailurus;


/// <summary>
/// Manages bookmarks in anonymous mode, where bookmarks are loaded from a file but not persisted between sessions.
/// </summary>
public class AnonymousBookmarkManager : IBookmarkManager
{
    private readonly List<BookmarkItem> _bookmarks = new();
    private BookmarkItem? _selectedBookmark;

    /// <summary>
    /// Initializes a new instance of the <see cref="AnonymousBookmarkManager"/> class.
    /// Automatically loads bookmarks from a file on instantiation.
    /// </summary>
    public AnonymousBookmarkManager()
    {
        LoadBookmarks();
    }

    /// <summary>
    /// Adds a new bookmark asynchronously.
    /// </summary>
    /// <param name="url">The URL of the bookmark.</param>
    /// <param name="title">The title of the bookmark.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task AddBookmarkAsync(string url, string title)
    {
        var bookmark = new BookmarkItem(url, title);
        _bookmarks.Add(bookmark);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Retrieves all bookmarks asynchronously.
    /// </summary>
    /// <returns>A <see cref="Task"/> containing an enumeration of bookmark URLs and titles.</returns>
    public Task<IEnumerable<(string Url, string Title)>> GetBookmarksAsync()
    {
        return Task.FromResult(_bookmarks.Select(b => (b.Url, b.Title)));
    }

    /// <summary>
    /// Removes a bookmark with the specified URL asynchronously.
    /// </summary>
    /// <param name="url">The URL of the bookmark to remove.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task RemoveBookmarkAsync(string url)
    {
        var bookmark = _bookmarks.FirstOrDefault(b => b.Url == url);
        if (bookmark != null)
        {
            _bookmarks.Remove(bookmark);
        }
        return Task.CompletedTask;
    }

    /// <summary>
    /// Retrieves the currently selected bookmark asynchronously.
    /// </summary>
    /// <returns>A <see cref="Task"/> containing the selected <see cref="BookmarkItem"/>, or null if none is selected.</returns>
    public Task<BookmarkItem?> GetSelectedBookmarkAsync()
    {
        return Task.FromResult(_selectedBookmark);
    }

    /// <summary>
    /// Selects a bookmark by its URL asynchronously.
    /// </summary>
    /// <param name="url">The URL of the bookmark to select.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task SelectBookmarkAsync(string url)
    {
        _selectedBookmark = _bookmarks.FirstOrDefault(b => b.Url == url);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Loads bookmarks from a file. This method is called during initialization.
    /// The bookmarks are loaded but not saved or modified afterward.
    /// </summary>
    private void LoadBookmarks()
    {
        const string bookmarksFilePath = "bookmarks.json";
        if (!File.Exists(bookmarksFilePath)) return;
        
        var json = File.ReadAllText(bookmarksFilePath);
        var bookmarksFromFile = System.Text.Json.JsonSerializer.Deserialize<List<BookmarkItem>>(json);
        
        if (bookmarksFromFile == null) return;
        
        _bookmarks.Clear();
        _bookmarks.AddRange(bookmarksFromFile);
    }
}