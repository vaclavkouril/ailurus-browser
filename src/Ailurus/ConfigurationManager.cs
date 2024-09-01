using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Avalonia.Input;

namespace Ailurus
{
    public class ConfigurationManager : IConfigurationManager
    {
        private const string ConfigFileName = "config.json";
        private ConfigurationData _configData;

        public ConfigurationManager()
        {
            var cfgData = LoadConfigFile();
            _configData = GetDefaultConfig();
            if (cfgData != null) _configData = cfgData;
        }

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

        public async Task<string> GetHomeUrlAsync()
        {
            return await Task.FromResult(_configData.General.HomeUrl);
        }

        public async Task SetKeyBindingAsync(string action, KeyGesture gesture)
        {
            _configData.KeyBindings[action] = gesture.ToString();
            await SaveConfigFileAsync();
        }

        public async Task SetHomeUrlAsync(string homeUrl)
        {
            _configData.General.HomeUrl = homeUrl;
            await SaveConfigFileAsync();
        }

        public async Task RefreshSettingsAsync()
        {
            _configData = LoadConfigFile() ?? GetDefaultConfig();
            await Task.CompletedTask;
        }

        private static ConfigurationData? LoadConfigFile()
        {
            if (!File.Exists(ConfigFileName)) return null;

            var json = File.ReadAllText(ConfigFileName);
            return JsonSerializer.Deserialize<ConfigurationData>(json);
        }

        private async Task SaveConfigFileAsync()
        {
            var json = JsonSerializer.Serialize(_configData, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(ConfigFileName, json);
        }

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

    public class ConfigurationData
    {
        public GeneralConfig General { get; set; } = new GeneralConfig();
        public Dictionary<string, string> KeyBindings { get; set; } = new Dictionary<string, string>();
    }

    public class GeneralConfig
    {
        public string HomeUrl { get; set; } = "https://www.google.com";
    }
}
