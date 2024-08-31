namespace Ailurus;

public class BookmarkItem(string url, string title)
{
    public string Url { get; } = url;
    public string Title { get; } = title;
}