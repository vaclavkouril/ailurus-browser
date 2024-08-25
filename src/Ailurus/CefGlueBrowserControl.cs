using System;
using Avalonia.Controls;
using Xilium.CefGlue;
using Xilium.CefGlue.Avalonia;
using Ailurus.ViewModels;
using Xilium.CefGlue.Common.Events;

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
            _tabViewModel = tabViewModel ?? throw new ArgumentNullException(nameof(tabViewModel));
        }

        /// <summary>
        /// Navigates the browser to the specified URL.
        /// </summary>
        /// <param name="url">The URL to navigate to.</param>
        public void Navigate(string url)
        {
            EnsureInitialized();
            _browser.Address = url;
        }

        /// <summary>
        /// Reloads the current page in the browser.
        /// </summary>
        public void Reload()
        {
            EnsureInitialized();
            _browser.Reload();
        }

        /// <summary>
        /// Navigates the browser to the previous page in the history, if available.
        /// </summary>
        public void GoBack()
        {
            EnsureInitialized();
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
            EnsureInitialized();
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
            if (_tabViewModel != null)
            {
                _tabViewModel.IsLoading = e.IsLoading;
            }
        }

        /// <summary>
        /// Handles the event when the browser navigates to a new URL.
        /// </summary>
        private void OnAddressChanged(object sender, string address)
        {
            if (_tabViewModel != null)
            {
                _tabViewModel.Url = address;
            }
        }

        /// <summary>
        /// Handles the event when the page title changes.
        /// </summary>
        private void OnTitleChanged(object sender, string title)
        {
            if (_tabViewModel != null)
            {
                _tabViewModel.Header = title;
            }
        }

        /// <summary>
        /// Handles navigation errors.
        /// </summary>
        private void OnNavigationError(object sender, LoadErrorEventArgs e)
        {
            // Handle navigation errors, like displaying an error page or message.
            // This is a placeholder; you can customize it as needed.
            Console.WriteLine($"Navigation error: {e.ErrorCode}, {e.ErrorText}");
        }

        /// <summary>
        /// Switches the browser content to the content of the selected tab.
        /// </summary>
        public void SwitchTab(BrowserTabViewModel tabViewModel)
        {
            EnsureInitialized();
            _tabViewModel = tabViewModel;
            _browser.Address = tabViewModel.Url;
        }

        /// <summary>
        /// Ensures the control is initialized before performing operations.
        /// </summary>
        private void EnsureInitialized()
        {
            if (_tabViewModel == null)
            {
                throw new InvalidOperationException("CefGlueBrowserControl must be initialized with a BrowserTabViewModel before use.");
            }
        }
    }
}
