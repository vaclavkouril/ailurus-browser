using Avalonia.Controls;

namespace Ailurus
{
    public class BrowserTab : TabItem
    {
        private CefGlueBrowserControl _browserControl;

        public BrowserTab(string initialUrl)
        {
            Header = initialUrl;
            _browserControl = new CefGlueBrowserControl();
            Content = _browserControl;
            Navigate(initialUrl);
        }

        public void Navigate(string url)
        {
            _browserControl.Navigate(url);
            Header = url; // Update the tab header to the current URL
        }

        public void GoBack() => _browserControl.GoBack();

        public void GoForward() => _browserControl.GoForward();
    }
}
