using System;
using Avalonia.Controls;
using Xilium.CefGlue;
using Xilium.CefGlue.Avalonia;

namespace Ailurus
{
    public class CefGlueBrowserControl : Control
    {
        private AvaloniaCefBrowser _browser;

        public CefGlueBrowserControl()
        {
            _browser = new AvaloniaCefBrowser();
            this.LogicalChildren.Add(_browser);
        }

        public void Navigate(string url)
        {
            if (_browser != null)
            {
                _browser.Address = url;
            }
        }
    }
}

