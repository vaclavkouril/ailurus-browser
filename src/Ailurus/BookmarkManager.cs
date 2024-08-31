using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ailurus;

public class BookmarkManager : IBookmarkManager
{
    private const string BookmarksFilePath = "bookmarks.json";
    private readonly List<BookmarkItem> _bookmarks = [];
    private BookmarkItem? _selectedBookmark;

    public async Task AddBookmarkAsync(string url, string title)
    {
        var bookmark = new BookmarkItem(url, title);
        _bookmarks.Add(bookmark);
        await SaveBookmarksToFileAsync();
    }

    public async Task<IEnumerable<(string Url, string Title)>> GetBookmarksAsync()
    {
        await LoadBookmarksFromFileAsync();
        return _bookmarks.Select(b => (b.Url, b.Title));
    }

    public async Task RemoveBookmarkAsync(string url)
    {
        var bookmark = _bookmarks.FirstOrDefault(b => b.Url == url);
        if (bookmark != null)
        {
            _bookmarks.Remove(bookmark);
            await SaveBookmarksToFileAsync();
        }
    }

    public async Task<BookmarkItem?> GetSelectedBookmarkAsync()
    {
        await LoadBookmarksFromFileAsync();
        return _selectedBookmark;
    }

    public async Task SelectBookmarkAsync(string url)
    {
        _selectedBookmark = _bookmarks.FirstOrDefault(b => b.Url == url);
        await SaveBookmarksToFileAsync(); // Save selection
    }

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

    private async Task SaveBookmarksToFileAsync()
    {
        var json = JsonSerializer.Serialize(_bookmarks);
        await File.WriteAllTextAsync(BookmarksFilePath, json);
    }
}