using Avalonia.Controls;
using Xilium.CefGlue;
using Xilium.CefGlue.Avalonia;
using Ailurus.ViewModels;
using Xilium.CefGlue.Common.Events;
using System;

namespace Ailurus
{
    public class CefGlueBrowserControl : Control
    {
        private AvaloniaCefBrowser _browser;
        private BrowserTabViewModel _tabViewModel;

        public CefGlueBrowserControl()
        {
            _browser = new AvaloniaCefBrowser();
            LogicalChildren.Add(_browser);

            // Attach event handlers
            _browser.LoadingStateChange += OnLoadingStateChanged;
            _browser.AddressChanged += OnAddressChanged;
            _browser.TitleChanged += OnTitleChanged;
        }

        /// <summary>
        /// Sets the BrowserTabViewModel after construction.
        /// </summary>
        public void Initialize(BrowserTabViewModel tabViewModel)
        {
            _tabViewModel = tabViewModel;
        }

        /// <summary>
        /// Navigates the browser to the specified URL.
        /// </summary>
        /// <param name="url">The URL to navigate to.</param>
        public void Navigate(string url)
        {
            _browser.Address = url;
        }

        /// <summary>
        /// Reloads the current page in the browser.
        /// </summary>
        public void Reload()
        {
            _browser.Reload();
        }

        /// <summary>
        /// Navigates the browser to the previous page in the history, if available.
        /// </summary>
        public void GoBack()
        {
            if (_browser.CanGoBack)
            {
                _browser.GoBack();
            }
        }

        /// <summary>
        /// Navigates the browser to the next page in the history, if available.
        /// </summary>
        public void GoForward()
        {
            if (_browser.CanGoForward)
            {
                _browser.GoForward();
            }
        }

        /// <summary>
        /// Handles the event when the loading state changes.
        /// </summary>
        private void OnLoadingStateChanged(object sender, LoadingStateChangeEventArgs e)
        {
            // Example: Update the tab's IsLoading property
            _tabViewModel.IsLoading = e.IsLoading;
        }

        /// <summary>
        /// Handles the event when the browser navigates to a new URL.
        /// </summary>
        private void OnAddressChanged(object sender, string address)
        {
            // Update the tab's URL property
            _tabViewModel.Url = address;
        }

        /// <summary>
        /// Handles the event when the page title changes.
        /// </summary>
        private void OnTitleChanged(object sender, string title)
        {
            // Update the tab's title
            _tabViewModel.Header = title;
        }

        /// <summary>
        /// Switches the browser content to the content of the selected tab.
        /// </summary>
        public void SwitchTab(BrowserTabViewModel tabViewModel)
        {
            // Logic to switch to the selected tab
            _tabViewModel = tabViewModel;
            _browser.Address = tabViewModel.Url;
        }
    }
}
