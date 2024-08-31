using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ailurus;

public class AnonymousHistoryManager : IHistoryManager
{
    public Task AddToHistoryAsync(string url)
    {
        return Task.CompletedTask;
    }

    public Task<IEnumerable<HistoryItem>> GetHistoryAsync()
    {
        return Task.FromResult<IEnumerable<HistoryItem>>(new List<HistoryItem>());
    }

    public Task DeleteHistoryAsync()
    {
        return Task.CompletedTask;
    }
}