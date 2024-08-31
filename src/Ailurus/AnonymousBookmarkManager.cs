using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Ailurus;

public class AnonymousBookmarkManager : IBookmarkManager
{
    private readonly List<BookmarkItem> _bookmarks = [];
    private BookmarkItem? _selectedBookmark;

    public AnonymousBookmarkManager()
    {
        LoadBookmarks();
    }

    public Task AddBookmarkAsync(string url, string title)
    {
        var bookmark = new BookmarkItem(url, title);
        _bookmarks.Add(bookmark);
        return Task.CompletedTask;
    }

    public Task<IEnumerable<(string Url, string Title)>> GetBookmarksAsync()
    {
        return Task.FromResult(_bookmarks.Select(b => (b.Url, b.Title)));
    }

    public Task RemoveBookmarkAsync(string url)
    {
        var bookmark = _bookmarks.FirstOrDefault(b => b.Url == url);
        if (bookmark != null)
        {
            _bookmarks.Remove(bookmark);
        }
        return Task.CompletedTask;
    }

    public Task<BookmarkItem?> GetSelectedBookmarkAsync()
    {
        return Task.FromResult(_selectedBookmark);
    }

    public Task SelectBookmarkAsync(string url)
    {
        _selectedBookmark = _bookmarks.FirstOrDefault(b => b.Url == url);
        return Task.CompletedTask;
    }

    private void LoadBookmarks()
    {
        // Load the bookmarks from the regular file without saving changes
        const string bookmarksFilePath = "bookmarks.json";
        if (!File.Exists(bookmarksFilePath)) return;
        
        var json = File.ReadAllText(bookmarksFilePath);
        var bookmarksFromFile = System.Text.Json.JsonSerializer.Deserialize<List<BookmarkItem>>(json);
        
        if (bookmarksFromFile == null) return;
        
        _bookmarks.Clear();
        _bookmarks.AddRange(bookmarksFromFile);
    }
}