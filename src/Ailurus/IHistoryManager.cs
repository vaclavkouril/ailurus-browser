using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ailurus
{
    public interface IHistoryManager
    {
        Task AddToHistoryAsync(string url);
        Task<IEnumerable<HistoryItem>> GetHistoryAsync();
        Task DeleteHistoryAsync();
    }
}