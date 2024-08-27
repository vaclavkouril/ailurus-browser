using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Xilium.CefGlue.Avalonia;

namespace Ailurus
{
    public class CefGlueBrowserControl : Control
    {
        private readonly AvaloniaCefBrowser _browser;

        public CefGlueBrowserControl(AvaloniaCefBrowser browser)
        {
            _browser = browser ?? throw new ArgumentNullException(nameof(browser));
            InitializeControl();
        }

        private void InitializeControl()
        {
            LogicalChildren.Add(_browser);
        }

        public async Task NavigateAsync(string url)
        {
            string normalizedUrl = NormalizeUrl(url);
            _browser.Address = normalizedUrl;
            await Task.CompletedTask;
        }

        public void Reload()
        {
            _browser.Reload();
        }

        public void GoBack()
        {
            if (_browser.CanGoBack) _browser.GoBack();
        }

        public void GoForward()
        {
            if (_browser.CanGoForward) _browser.GoForward();
        }

        public void OpenDevTools()
        {
            _browser.ShowDeveloperTools();
        }
        
        public string CurrentUrl
        {
            get => _browser.Address;
            set => _browser.Address = value;
        }
        
        public string Title => _browser.Title;

        private string NormalizeUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return "about:blank";
            }

            url = url.Trim();

            return url switch
            {
                _ when url.StartsWith("about:", StringComparison.OrdinalIgnoreCase) => url,
                _ when url.StartsWith("file://", StringComparison.OrdinalIgnoreCase) => url,
                _ when Uri.IsWellFormedUriString(url, UriKind.Absolute) => url,
                _ when IsLikelyHostOrIp(url) => $"https://{url}",
                _ when url.Contains('.') && !url.Contains(" ") => $"https://{url}",
                _ => $"https://www.google.com/search?q={Uri.EscapeDataString(url)}"
            };
        }

        private bool IsLikelyHostOrIp(string url)
        {
            if (IsIpAddress(url))
            {
                return true;
            }
            
            if (Uri.CheckHostName(url) == UriHostNameType.Dns)
            {
                return url.Contains('.') && !url.StartsWith('.') && !url.EndsWith('.');
            }

            return false;
        }
        private bool IsIpAddress(string url) => System.Net.IPAddress.TryParse(url, out _);

    }
}
