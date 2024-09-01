namespace Ailurus;

/// <summary>
/// Represents a bookmark item with a URL and a title.
/// </summary>
/// <param name="url">The URL of the bookmark.</param>
/// <param name="title">The title of the bookmark.</param>
public class BookmarkItem(string url, string title)
{
    public string Url { get; } = url;
    public string Title { get; } = title;
}