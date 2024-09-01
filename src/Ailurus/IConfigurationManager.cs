using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Input;

public interface IConfigurationManager
{
    Task<Dictionary<string, KeyGesture>> GetKeyBindingsAsync();
    Task SetKeyBindingAsync(string action, KeyGesture gesture);
    Task<string> GetHomeUrlAsync();
    Task SetHomeUrlAsync(string url);
    Task RefreshSettingsAsync();
}