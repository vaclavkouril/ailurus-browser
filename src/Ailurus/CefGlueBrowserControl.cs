using Avalonia.Controls;
using Xilium.CefGlue.Avalonia;

namespace Ailurus
{
    public class CefGlueBrowserControl : Control
    {
        private AvaloniaCefBrowser _browser;

        public CefGlueBrowserControl()
        {
            _browser = new AvaloniaCefBrowser();
            LogicalChildren.Add(_browser);
        }

        public void Navigate(string url)
        {
            _browser.Address = url;
        }

        public void GoBack()
        {
            if (_browser.CanGoBack)
            {
                _browser.GoBack();
            }
        }

        public void GoForward()
        {
            if (_browser.CanGoForward)
            {
                _browser.GoForward();
            }
        }

        public string Address => _browser.Address;
    }
}
