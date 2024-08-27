using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ailurus
{
    public interface IBookmarkManager
    {
        Task AddBookmarkAsync(string url, string title);
        Task<IEnumerable<(string Url, string Title)>> GetBookmarksAsync();
        Task RemoveBookmarkAsync(string url);
    }
}