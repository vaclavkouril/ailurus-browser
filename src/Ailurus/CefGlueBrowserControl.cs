using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Xilium.CefGlue.Avalonia;
using Ailurus.ViewModels;

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
            LogicalChildren.Add(_browser); // Add the browser as a visual child
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

        public string Title { get => _browser.Title; }
        
        private string NormalizeUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return "about:blank";
            }

            url = url.Trim();

            if (!url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
                !url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                // If the URL does not start with http:// or https://, add https:// by default.
                url = "https://" + url;
            }

            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                // If the URL is not well-formed, assume it's a search query.
                url = "https://www.google.com/search?q=" + Uri.EscapeDataString(url);
            }

            return url;
        }
    }
}