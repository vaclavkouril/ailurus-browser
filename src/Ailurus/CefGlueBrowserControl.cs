using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using Xilium.CefGlue;
using Xilium.CefGlue.Avalonia;
using Ailurus.ViewModels;
using Xilium.CefGlue.Common.Events;

namespace Ailurus
{
    public class CefGlueBrowserControl : Control
    {
        private AvaloniaCefBrowser? _browser; // Marked as nullable
        private BrowserTabViewModel? _tabViewModel; // Marked as nullable
        private readonly TaskFactory _uiTaskFactory;
        private bool _isBrowserInitialized = false;

        public CefGlueBrowserControl()
        {
            _tabViewModel = null;
            _uiTaskFactory = new TaskFactory(TaskScheduler.FromCurrentSynchronizationContext());
            InitializeBrowserAsync();
        }

        private void InitializeBrowserAsync()
        {
            _uiTaskFactory.StartNew(() =>
            {
                _browser = new AvaloniaCefBrowser();
                _browser.LoadingStateChange += OnLoadingStateChanged;
                _browser.AddressChanged += OnAddressChanged;
                _browser.TitleChanged += OnTitleChanged;

                LogicalChildren.Add(_browser);
                _isBrowserInitialized = true; // Mark the browser as initialized
            });
        }

        public void Initialize(BrowserTabViewModel tabViewModel)
        {
            _tabViewModel = tabViewModel ?? throw new ArgumentNullException(nameof(tabViewModel));
        }

        public async Task NavigateAsync(string url)
        {
            try
            {
                await EnsureInitializedAsync();
                await _uiTaskFactory.StartNew(() => _browser!.Address = url); // Use null-forgiving operator
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Navigation failed: {ex.Message}");
            }
        }

        public async Task ReloadAsync()
        {
            await EnsureInitializedAsync();
            await _uiTaskFactory.StartNew(() => _browser!.Reload()); // Use null-forgiving operator
        }

        public async Task GoBackAsync()
        {
            await EnsureInitializedAsync();
            await _uiTaskFactory.StartNew(() =>
            {
                if (_browser!.CanGoBack) _browser.GoBack(); // Use null-forgiving operator
            });
        }

        public async Task GoForwardAsync()
        {
            await EnsureInitializedAsync();
            await _uiTaskFactory.StartNew(() =>
            {
                if (_browser!.CanGoForward) _browser.GoForward(); // Use null-forgiving operator
            });
        }

        private void OnLoadingStateChanged(object sender, LoadingStateChangeEventArgs e)
        {
            if (_tabViewModel != null)
            {
                Dispatcher.UIThread.Post(() => _tabViewModel.IsLoading = e.IsLoading);
            }
        }

        private void OnAddressChanged(object sender, string address)
        {
            if (_tabViewModel != null)
            {
                Dispatcher.UIThread.Post(() => _tabViewModel.Url = address);
            }
        }

        private void OnTitleChanged(object sender, string title)
        {
            if (_tabViewModel != null)
            {
                Dispatcher.UIThread.Post(() => _tabViewModel.Header = title);
            }
        }

        private void OnNavigationError(object sender, LoadErrorEventArgs e)
        {
            Console.WriteLine($"Navigation error: {e.ErrorCode}, {e.ErrorText}");
        }

        public async Task SwitchTabAsync(BrowserTabViewModel tabViewModel)
        {
            await EnsureInitializedAsync();
            await _uiTaskFactory.StartNew(() =>
            {
                _tabViewModel = tabViewModel;
                _browser!.Address = tabViewModel.Url; // Use null-forgiving operator
            });
        }

        private async Task EnsureInitializedAsync()
        {
            if (_tabViewModel == null || !_isBrowserInitialized || _browser == null)
            {
                throw new InvalidOperationException("CefGlueBrowserControl must be initialized with a BrowserTabViewModel and the browser must be fully loaded before use.");
            }
            await Task.CompletedTask;
        }
    }
}
