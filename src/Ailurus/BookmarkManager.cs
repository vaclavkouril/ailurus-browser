using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ailurus;

/// <summary>
/// Manages bookmarks, including loading from and saving to a file.
/// </summary>
public class BookmarkManager : IBookmarkManager
{
    private const string BookmarksFilePath = "bookmarks.json";
    private readonly List<BookmarkItem> _bookmarks = new();
    private BookmarkItem? _selectedBookmark;

    /// <summary>
    /// Adds a new bookmark and saves it to the file.
    /// </summary>
    /// <param name="url">The URL of the bookmark.</param>
    /// <param name="title">The title of the bookmark.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task AddBookmarkAsync(string url, string title)
    {
        var bookmark = new BookmarkItem(url, title);
        _bookmarks.Add(bookmark);
        await SaveBookmarksToFileAsync();
    }

    /// <summary>
    /// Retrieves all bookmarks by loading them from the file.
    /// </summary>
    /// <returns>A <see cref="Task"/> containing an enumeration of bookmark URLs and titles.</returns>
    public async Task<IEnumerable<(string Url, string Title)>> GetBookmarksAsync()
    {
        await LoadBookmarksFromFileAsync();
        return _bookmarks.Select(b => (b.Url, b.Title));
    }

    /// <summary>
    /// Removes a bookmark by its URL and saves the updated list to the file.
    /// </summary>
    /// <param name="url">The URL of the bookmark to remove.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task RemoveBookmarkAsync(string url)
    {
        var bookmark = _bookmarks.FirstOrDefault(b => b.Url == url);
        if (bookmark != null)
        {
            _bookmarks.Remove(bookmark);
            await SaveBookmarksToFileAsync();
        }
    }

    /// <summary>
    /// Retrieves the currently selected bookmark.
    /// </summary>
    /// <returns>A <see cref="Task"/> containing the selected <see cref="BookmarkItem"/>, or null if none is selected.</returns>
    public async Task<BookmarkItem?> GetSelectedBookmarkAsync()
    {
        await LoadBookmarksFromFileAsync();
        return _selectedBookmark;
    }

    /// <summary>
    /// Selects a bookmark by its URL and saves the selection.
    /// </summary>
    /// <param name="url">The URL of the bookmark to select.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task SelectBookmarkAsync(string url)
    {
        _selectedBookmark = _bookmarks.FirstOrDefault(b => b.Url == url);
        await SaveBookmarksToFileAsync(); // Save selection
    }

    /// <summary>
    /// Loads bookmarks from a file asynchronously.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    private async Task LoadBookmarksFromFileAsync()
    {
        if (File.Exists(BookmarksFilePath))
        {
            var json = await File.ReadAllTextAsync(BookmarksFilePath);
            var bookmarksFromFile = JsonSerializer.Deserialize<List<BookmarkItem>>(json);
            if (bookmarksFromFile != null)
            {
                _bookmarks.Clear();
                _bookmarks.AddRange(bookmarksFromFile);
            }
        }
    }

    /// <summary>
    /// Saves the current list of bookmarks to a file asynchronously.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    private async Task SaveBookmarksToFileAsync()
    {
        var json = JsonSerializer.Serialize(_bookmarks);
        await File.WriteAllTextAsync(BookmarksFilePath, json);
    }
}