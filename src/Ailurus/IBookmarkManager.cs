using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ailurus;

/// <summary>
/// Interface for managing bookmarks within the browser application.
/// </summary>
public interface IBookmarkManager
{
    /// <summary>
    /// Adds a new bookmark with the specified URL and title.
    /// </summary>
    /// <param name="url">The URL of the bookmark.</param>
    /// <param name="title">The title of the bookmark.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task AddBookmarkAsync(string url, string title);

    /// <summary>
    /// Retrieves all bookmarks.
    /// </summary>
    /// <returns>A <see cref="Task"/> containing a collection of tuples representing the URL and title of each bookmark.</returns>
    Task<IEnumerable<(string Url, string Title)>> GetBookmarksAsync();

    /// <summary>
    /// Removes the bookmark with the specified URL.
    /// </summary>
    /// <param name="url">The URL of the bookmark to remove.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task RemoveBookmarkAsync(string url);

    /// <summary>
    /// Gets the currently selected bookmark.
    /// </summary>
    /// <returns>A <see cref="Task"/> containing the currently selected <see cref="BookmarkItem"/> if one exists; otherwise, null.</returns>
    Task<BookmarkItem?> GetSelectedBookmarkAsync();

    /// <summary>
    /// Selects a bookmark with the specified URL.
    /// </summary>
    /// <param name="url">The URL of the bookmark to select.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task SelectBookmarkAsync(string url);
}