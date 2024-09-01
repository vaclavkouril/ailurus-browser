using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Avalonia.Input;

namespace Ailurus;

/// <summary>
/// Manages configuration settings including key bindings and the home URL.
/// </summary>
public class ConfigurationManager : IConfigurationManager
{
    private const string ConfigFileName = "config.json";
    private ConfigurationData _configData;

    /// <summary>
    /// Initializes a new instance of <see cref="ConfigurationManager"/> and loads the configuration file, 
    /// or uses default settings if the file is not found.
    /// </summary>
    public ConfigurationManager()
    {
        var cfgData = LoadConfigFile();
        _configData = GetDefaultConfig();
        if (cfgData != null) _configData = cfgData;
    }

    /// <summary>
    /// Retrieves the key bindings from the configuration.
    /// </summary>
    /// <returns>A dictionary mapping actions to their respective <see cref="KeyGesture"/>.</returns>
    public async Task<Dictionary<string, KeyGesture>> GetKeyBindingsAsync()
    {
        var keyBindings = new Dictionary<string, KeyGesture>();

        if (_configData.KeyBindings == null) return await Task.FromResult(keyBindings);
            
        foreach (var kvp in _configData.KeyBindings)
        {
            keyBindings[kvp.Key] = ParseKeyGesture(kvp.Value);
        }

        return await Task.FromResult(keyBindings);
    }

    /// <summary>
    /// Retrieves the home URL from the configuration.
    /// </summary>
    /// <returns>The home URL as a string.</returns>
    public async Task<string> GetHomeUrlAsync()
    {
        return await Task.FromResult(_configData.General.HomeUrl);
    }

    /// <summary>
    /// Sets a key binding for a specific action and saves the configuration.
    /// </summary>
    /// <param name="action">The action to associate with the key binding.</param>
    /// <param name="gesture">The key gesture representing the binding.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task SetKeyBindingAsync(string action, KeyGesture gesture)
    {
        _configData.KeyBindings[action] = gesture.ToString();
        await SaveConfigFileAsync();
    }

    /// <summary>
    /// Sets the home URL and saves the configuration.
    /// </summary>
    /// <param name="homeUrl">The new home URL.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task SetHomeUrlAsync(string homeUrl)
    {
        _configData.General.HomeUrl = homeUrl;
        await SaveConfigFileAsync();
    }

    /// <summary>
    /// Refreshes the settings by reloading the configuration file.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task RefreshSettingsAsync()
    {
        _configData = LoadConfigFile() ?? GetDefaultConfig();
        await Task.CompletedTask;
    }

    /// <summary>
    /// Loads the configuration data from the file.
    /// </summary>
    /// <returns>The <see cref="ConfigurationData"/> object if the file is found, otherwise null.</returns>
    private static ConfigurationData? LoadConfigFile()
    {
        if (!File.Exists(ConfigFileName)) return null;

        var json = File.ReadAllText(ConfigFileName);
        return JsonSerializer.Deserialize<ConfigurationData>(json);
    }

    /// <summary>
    /// Saves the current configuration data to the configuration file.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    private async Task SaveConfigFileAsync()
    {
        var json = JsonSerializer.Serialize(_configData, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(ConfigFileName, json);
    }

    /// <summary>
    /// Provides the default configuration settings.
    /// </summary>
    /// <returns>A <see cref="ConfigurationData"/> object with default values.</returns>
    private static ConfigurationData GetDefaultConfig()
    {
        return new ConfigurationData
        {
            General = new GeneralConfig
            {
                HomeUrl = "https://www.google.com"
            },
            KeyBindings = new Dictionary<string, string>
            {
                { "NavigateUp", "Ctrl+Up" },
                { "NavigateDown", "Ctrl+Down" },
                { "AddBookmark", "Ctrl+B" },
                { "OpenHistory", "Ctrl+H" },
                { "Go", "Ctrl+G" },
                { "Back", "Ctrl+Left" },
                { "Forward", "Ctrl+Right" },
                { "Reload", "Ctrl+R" }
            }
        };
    }

    /// <summary>
    /// Parses a key gesture string into a <see cref="KeyGesture"/> object.
    /// </summary>
    /// <param name="gesture">The string representation of the key gesture.</param>
    /// <returns>The parsed <see cref="KeyGesture"/> object.</returns>
    /// <exception cref="ArgumentException">Thrown if the key gesture string is invalid.</exception>
    private KeyGesture ParseKeyGesture(string gesture)
    {
        var gestureParts = gesture.Split('+');
        var modifierMap = new Dictionary<string, KeyModifiers>
        {
            { "Ctrl", KeyModifiers.Control },
            { "Control", KeyModifiers.Control },
            { "Shift", KeyModifiers.Shift },
            { "Alt", KeyModifiers.Alt },
            { "Win", KeyModifiers.Meta },
            { "Cmd", KeyModifiers.Meta },
            { "Meta", KeyModifiers.Meta }
        };

        var modifiers = gestureParts.Length > 1
            ? gestureParts[..^1]
                .Select(s => modifierMap.ContainsKey(s) ? modifierMap[s] : throw new ArgumentException($"Invalid modifier: {s}"))
                .Aggregate((a, b) => a | b)
            : KeyModifiers.None;

        var key = Enum.Parse<Key>(gestureParts[^1], true);

        return new KeyGesture(key, modifiers);
    }

}

/// <summary>
/// Represents the configuration data structure, including general settings and key bindings.
/// </summary>
public class ConfigurationData
{
    public GeneralConfig General { get; set; } = new GeneralConfig();
    public Dictionary<string, string> KeyBindings { get; set; } = new Dictionary<string, string>();
}

/// <summary>
/// Represents general configuration settings, such as the home URL.
/// </summary>
public class GeneralConfig
{
    public string HomeUrl { get; set; } = "https://www.google.com";
}