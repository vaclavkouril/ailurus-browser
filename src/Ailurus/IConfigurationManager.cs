using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Input;

namespace Ailurus;

/// <summary>
/// Interface for managing application configuration settings, including key bindings and the home URL.
/// </summary>
public interface IConfigurationManager
{
    /// <summary>
    /// Retrieves the key bindings for various actions.
    /// </summary>
    /// <returns>A <see cref="Task"/> containing a dictionary mapping action names to <see cref="KeyGesture"/> objects.</returns>
    Task<Dictionary<string, KeyGesture>> GetKeyBindingsAsync();

    /// <summary>
    /// Sets a key binding for a specific action.
    /// </summary>
    /// <param name="action">The name of the action to bind.</param>
    /// <param name="gesture">The <see cref="KeyGesture"/> representing the key binding.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task SetKeyBindingAsync(string action, KeyGesture gesture);

    /// <summary>
    /// Retrieves the home URL set in the configuration.
    /// </summary>
    /// <returns>A <see cref="Task"/> containing the home URL as a string.</returns>
    Task<string> GetHomeUrlAsync();

    /// <summary>
    /// Sets the home URL in the configuration.
    /// </summary>
    /// <param name="url">The new home URL.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task SetHomeUrlAsync(string url);

    /// <summary>
    /// Refreshes the configuration settings by reloading them from the configuration file.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task RefreshSettingsAsync();
}