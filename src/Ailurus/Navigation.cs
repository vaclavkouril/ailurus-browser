using Avalonia.Controls;

namespace Ailurus
{
    public class BrowserTab : TabItem
    {
        private readonly CefGlueBrowserControl _browserControl;

        public BrowserTab(string initialUrl)
        {
            Header = initialUrl;
            _browserControl = new CefGlueBrowserControl();
            Content = _browserControl;
            Navigate(initialUrl);
        }

        public async void Navigate(string url)
        {
            await _browserControl.NavigateAsync(url);
            Header = url; // Update the tab header to the current URL
        }

        public async void GoBack() => await _browserControl.GoBackAsync();

        public async void GoForward() => await _browserControl.GoForwardAsync();
    }
}