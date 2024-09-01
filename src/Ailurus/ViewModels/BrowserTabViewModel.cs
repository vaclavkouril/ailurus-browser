using System.Reactive;
using ReactiveUI;
using System.Threading.Tasks;
using Xilium.CefGlue.Avalonia;

namespace Ailurus.ViewModels
{
    /// <summary>
    /// ViewModel representing a single browser tab.
    /// </summary>
    public class BrowserTabViewModel : ReactiveObject
    {
        private string _header = "New Tab";
        private string _url = "https://www.google.com";
        private bool _isLoading;
        private bool _isSelected;

        /// <summary>
        /// Gets or sets the tab header, usually the page title.
        /// </summary>
        public string Header
        {
            get => _header;
            private set => this.RaiseAndSetIfChanged(ref _header, value);
        }

        /// <summary>
        /// Gets or sets the current URL of the browser tab.
        /// </summary>
        public string Url
        {
            get => BrowserControl.CurrentUrl;
            set
            {
                if (_url == value) return;
                
                this.RaiseAndSetIfChanged(ref _url, value);
                NavigateAsync(value).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the browser is currently loading.
        /// </summary>
        public bool IsLoading
        {
            get => _isLoading;
            set => this.RaiseAndSetIfChanged(ref _isLoading, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the tab is currently selected.
        /// </summary>
        public bool IsSelected
        {
            get => _isSelected;
            set => this.RaiseAndSetIfChanged(ref _isSelected, value);
        }

        /// <summary>
        /// Gets the AvaloniaCefBrowser instance for this tab.
        /// </summary>
        public AvaloniaCefBrowser Browser { get; }

        /// <summary>
        /// Gets the CefGlueBrowserControl instance for managing browser operations.
        /// </summary>
        public CefGlueBrowserControl BrowserControl { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BrowserTabViewModel"/> class.
        /// </summary>
        /// <param name="mainWindowViewModel">The main window view model.</param>
        public BrowserTabViewModel(MainWindowViewModel mainWindowViewModel)
        {
            Browser = new AvaloniaCefBrowser();
            BrowserControl = new CefGlueBrowserControl(Browser);
            
            Browser.AddressChanged += OnBrowserAddressChanged;
            Browser.TitleChanged += OnBrowserTitleChanged;

            CloseTabCommand = ReactiveCommand.Create(() => mainWindowViewModel.CloseTab(this));
            SelectTabCommand = ReactiveCommand.Create(() =>
            {
                mainWindowViewModel.SelectedTab = this;
                return Unit.Default;
            });
        }

        /// <summary>
        /// Handles the AddressChanged event to update the URL property.
        /// </summary>
        private void OnBrowserAddressChanged(object sender, string e)
        {
            this.RaisePropertyChanged(nameof(Url));
        }

        /// <summary>
        /// Handles the TitleChanged event to update the Header property.
        /// </summary>
        private void OnBrowserTitleChanged(object sender, string newTitle)
        {
            Header = BrowserControl.Title;
        }

        /// <summary>
        /// Navigates the browser to the specified URL asynchronously.
        /// </summary>
        /// <param name="url">The URL to navigate to.</param>
        public async Task NavigateAsync(string url)
        {
            if (!string.IsNullOrWhiteSpace(url) && BrowserControl.CurrentUrl != url)
            {
                await BrowserControl.NavigateAsync(url);
            }
        }

        /// <summary>
        /// Sets the IsSelected property for this tab.
        /// </summary>
        /// <param name="isSelected">Whether the tab is selected.</param>
        public void SetIsSelected(bool isSelected)
        {
            IsSelected = isSelected;
        }

        /// <summary>
        /// Command to close the tab.
        /// </summary>
        public ReactiveCommand<Unit, Unit> CloseTabCommand { get; }

        /// <summary>
        /// Command to select the tab.
        /// </summary>
        public ReactiveCommand<Unit, Unit> SelectTabCommand { get; }
    }
}
